using Application.Dto;

namespace Client.Services
{
	public interface IQuizDataService
	{
		Task<IEnumerable<QuizBasicResponseDto>> GetQuizes();
	}
}
