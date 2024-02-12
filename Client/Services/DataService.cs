using Client.Pages;
using Newtonsoft.Json;
using Application.Dto;
using System.Net.Http.Json;

namespace Client.Services
{
	public class DataService(HttpClient httpClient) : IDataService
	{
		private readonly HttpClient _httpClient = httpClient;

		public async Task <IEnumerable<QuizBasicResponseDto>> GetQuizes()
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

		public async Task<System.Net.HttpStatusCode> RegisterAsync(string email , string password)
		{
			var body = new
			{
				email,
				password
			};

			var request = await _httpClient.PostAsJsonAsync("/api/register", body);

			return request.StatusCode;
		}
	}
}
