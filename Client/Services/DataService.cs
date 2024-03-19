using Client.Pages;
using Newtonsoft.Json;
using Application.Dto;
using System.Net.Http.Json;
using Client.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Services
{
	public class DataService(
		HttpClient httpClient,
		ILocalStorageService localStorageService,
		CustomAuthStateProvider authenticationStateProvider 
		) : IDataService
	{
		private readonly HttpClient _httpClient = httpClient;
		private readonly ILocalStorageService _localStorageService = localStorageService;
		private readonly CustomAuthStateProvider _authenticationStateProvider = authenticationStateProvider;

		public async Task<IEnumerable<QuizBasicResponseDto>> GetQuizes()
		{
			var result = await _httpClient.GetAsync("api/quizes");

			var data = JsonConvert.DeserializeObject<IEnumerable<QuizBasicResponseDto>>
				(await result.Content.ReadAsStringAsync());

			return data!;
		}

		public async Task<QuizDetailResponseDto> GetQuizDetailAsync(string quizId)
		{
			Guid quizIdAsGuid = Guid.Parse(quizId);

			var result = await _httpClient.GetAsync($"api/quizes/{quizIdAsGuid}");

			var data = JsonConvert.DeserializeObject<QuizDetailResponseDto>
				(await result.Content.ReadAsStringAsync());

			return data!;
		}

		public async Task<System.Net.HttpStatusCode> RegisterAsync(string email, string password)
		{
			var body = new
			{
				email,
				password
			};

			var request = await _httpClient.PostAsJsonAsync("/api/register", body);

			return request.StatusCode;
		}

		public async Task<bool> LoginAsync(string email, string password)
		{
			var body = new
			{
				email,
				password
			};

			var request = await _httpClient.PostAsJsonAsync("/api/login", body);

			if (!request.IsSuccessStatusCode)
			{
				return false;
			}

			var response = JsonConvert.DeserializeObject<TokenResponse>
				(await request.Content.ReadAsStringAsync());

			await _localStorageService.SetItemAsStringAsync("accessToken", response!.AccessToken);
			await _localStorageService.SetItemAsStringAsync("refreshToken", response!.RefreshToken);

			await _authenticationStateProvider.AuthenticateAsync(new User(email));

			return true;
		}

		public async Task<IEnumerable<QuizBasicResponseDto>> GetUserQuizesAsync()
		{
			var result = await _httpClient.GetAsync("api/user/quizes?CreationStatus=Succes");

			var data = JsonConvert.DeserializeObject<IEnumerable<QuizBasicResponseDto>>
				(await result.Content.ReadAsStringAsync());

			return data!;
		}

		public async Task<QuizDetailResponseDto> GetUserQuizDetailAsync(string quizId)
		{
			Guid quizIdAsGuid = Guid.Parse(quizId);

			var result = await _httpClient.GetAsync($"api/user/quizes/{quizIdAsGuid}");

			var data = JsonConvert.DeserializeObject<QuizDetailResponseDto>
				(await result.Content.ReadAsStringAsync());

			return data!;
		}

		public async Task<bool> SendCreateUserQuizRequestAsync(CreationQuizDataModel quizToCreateData)
		{
			var result = await _httpClient.PostAsJsonAsync("api/user/quizes" , quizToCreateData);

			return result.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteQuizAsync(Guid quizId)
		{
			var result = await _httpClient.DeleteAsync($"api/user/quizes/{quizId}");

			return result.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<AlgorithmTaskBasicResponseDto>> GetAlgorithmsAsync()
		{
			var result = await _httpClient.GetAsync("api/algorithms");

			var data = JsonConvert.DeserializeObject<IEnumerable<AlgorithmTaskBasicResponseDto>>
				(await result.Content.ReadAsStringAsync());

			return data!;
		}

		public async Task<AlgorithmTaskDetailResponseDto> GetAlgorithmDetailAsync(Guid algorithmId)
		{
			var result = await _httpClient.GetAsync($"api/algorithms/{algorithmId}");

			var data = JsonConvert.DeserializeObject<AlgorithmTaskDetailResponseDto>
				(await result.Content.ReadAsStringAsync());

			return data!;
		}

		public async Task<IEnumerable<AlgorithmTaskBasicResponseDto>> GetUserAlgorithmsAsync()
		{
			var result = await _httpClient.GetAsync("api/user/algorithms?CreationStatus=Succes");

			var data = JsonConvert.DeserializeObject<IEnumerable<AlgorithmTaskBasicResponseDto>>
				(await result.Content.ReadAsStringAsync());

			return data!;
		}

		public async Task<AlgorithmTaskDetailResponseDto> GetUserAlgorithmDetailAsync(Guid algorithmId)
		{
			var result = await _httpClient.GetAsync($"api/user/algorithms/{algorithmId}");

			var data = JsonConvert.DeserializeObject<AlgorithmTaskDetailResponseDto>
				(await result.Content.ReadAsStringAsync());

			return data!;
		}

		public async Task<bool> DeleteUserAlgorithmAsync(Guid algorithmId)
		{
			var result = await _httpClient.DeleteAsync($"api/user/algorithms/{algorithmId}");

			return result.IsSuccessStatusCode;
		}

		public async Task<bool> SendCreateAlgorithmReqeustAsync(CreationAlgorithmDataModel model)
		{
			var result = await _httpClient.PostAsJsonAsync("api/user/algorithms", model);

			return result.IsSuccessStatusCode;
		}
	}
}
