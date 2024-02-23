using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Client.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Client.Services
{
	public class AccessTokenRequestHandler(
		ILocalStorageService localStorageService,
		HttpClient httpClient,
		CustomAuthStateProvider authenticationStateProvider
		) : DelegatingHandler
	{
		private readonly ILocalStorageService _localStorageService = localStorageService;
		private readonly HttpClient _httpClient = httpClient;
		private readonly CustomAuthStateProvider _authenticationStateProvider = authenticationStateProvider;

		protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await GetAccessToken();

			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await base.SendAsync(request, cancellationToken);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				var refreshAction = await RefreshAsync();

				if (refreshAction == false)
				{
					await _authenticationStateProvider.LogOutAsync();
				}

				return await SendAsync(request, cancellationToken);
			}

			return response;
		}

		private async Task<bool> RefreshAsync()
		{
			var refreshToken = await GetRefreshToken();

			if (refreshToken == null) return false;

			var body = new
			{
				refreshToken
			};

			var request = await _httpClient.PostAsJsonAsync("/api/refresh", body);

			if (!request.IsSuccessStatusCode) return false;

			var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>
				(await request.Content.ReadAsStringAsync());

			if (tokenResponse == null) return false;

			await _localStorageService.SetItemAsStringAsync("accessToken", tokenResponse.AccessToken);
			await _localStorageService.SetItemAsStringAsync("refreshToken", tokenResponse.RefreshToken);

			return true;
		}

		private async Task<string?> GetAccessToken() => 
			await _localStorageService.GetItemAsStringAsync("accessToken");

		private async Task<string?> GetRefreshToken() =>
			await _localStorageService.GetItemAsStringAsync("refreshToken");
	}
}
