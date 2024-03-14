using Application.Dto;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class AiAlgorithmsConverter
	{
		public AlgorithmTask ConvertToAlgorithmTask(AlgorithmAiResponseDto response, string taskTitle, AdvanceNumber advanceNumber)
		{
			var id = Guid.NewGuid();

			return new(id, taskTitle, response.QuestionContent, advanceNumber)
			{
				Answers = GetAlgorithmAnswers(response.LanguageAiResponseDtos , id).ToList()
			};
		}

		private IEnumerable<AlgorithmAnswer> GetAlgorithmAnswers(IEnumerable<ProgrammingLanguageWithAnswerAiResponseDto> languageAnswersResponses , Guid taskId)
		{
			List<AlgorithmAnswer> algorithmAnswers = [];

			foreach (var languageAnswer in languageAnswersResponses)
			{
				algorithmAnswers.Add(new(Guid.NewGuid(), languageAnswer.Content, languageAnswer.LanguageName)
				{
					AlgorithmTaskId = taskId
				});
			}

			return algorithmAnswers;
		}
	}
}
