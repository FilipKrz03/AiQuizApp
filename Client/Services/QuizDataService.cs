using Client.Pages;
using Newtonsoft.Json;
using Application.Dto;

namespace Client.Services
{
	public class QuizDataService(HttpClient httpClient) : IQuizDataService
	{
		private readonly HttpClient _httpClient = httpClient;

		public async Task <IEnumerable<QuizBasicResponseDto>> GetQuizes()
		{
			var result = await _httpClient.GetAsync("api/quizes");

			var data = JsonConvert.DeserializeObject<IEnumerable<QuizBasicResponseDto>>
				(await result.Content.ReadAsStringAsync());

			return data!;
		}
	}
}
