using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Blazored.LocalStorage;
using Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Client.Services
{
	public class AccessTokenRequestHandler(
		ILocalStorageService localStorageService,
		CustomAuthStateProvider authenticationStateProvider , 
		NavigationManager navigationManager
		) : DelegatingHandler
	{
		private readonly ILocalStorageService _localStorageService = localStorageService;
		private readonly CustomAuthStateProvider _authenticationStateProvider = authenticationStateProvider;
		private readonly NavigationManager _navigationManager = navigationManager;

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
					_navigationManager.NavigateTo("/login");

					return response;
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

			var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/refresh");
			httpRequest.Content = new StringContent(JsonConvert.SerializeObject(body), new MediaTypeHeaderValue("application/json"));

			var request = await base.SendAsync(httpRequest , default);

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
