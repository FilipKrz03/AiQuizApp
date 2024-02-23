using Application.Dto;

namespace Client.Services
{
	public interface IDataService
	{
		Task<IEnumerable<QuizBasicResponseDto>> GetQuizes();
		Task<QuizDetailResponseDto> GetQuizDetailAsync(string quizId);
		Task<System.Net.HttpStatusCode> RegisterAsync(string email, string password);
		Task<bool> LoginAsync(string email, string password);
	}
}
