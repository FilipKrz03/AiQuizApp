using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Client.Models;

namespace Client.Services
{
	public class CustomAuthStateProvider(ILocalStorageService localStorageService)
		: AuthenticationStateProvider
	{
		private readonly ILocalStorageService _localStorageService = localStorageService;

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var userJson = await _localStorageService.GetItemAsStringAsync("user");

			var claims = new ClaimsIdentity();

			if (userJson != null)
			{
				var user = JsonSerializer.Deserialize<User>(userJson!);
				
				var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
				{
					new Claim( ClaimTypes.Name, user!.Email),
				}, "auth"));

				return await Task.FromResult(new AuthenticationState(principal));
			}

			return new AuthenticationState(new ClaimsPrincipal(claims));
		}

		public async Task AuthenticateAsync(User user)
		{
			var json = JsonSerializer.Serialize(user);
			await _localStorageService.SetItemAsStringAsync("user", json);

			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		public async Task LogOutAsync()
		{
			await _localStorageService.RemoveItemAsync("accessToken");
			await _localStorageService.RemoveItemAsync("refreshToken");
			await _localStorageService.RemoveItemAsync("user");

			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}
	}
}
