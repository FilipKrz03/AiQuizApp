using Domain.ValueObjects;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI.ObjectModels.RequestModels;
using Application.Props;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Application.Dto;
using Domain.Entities;

namespace Application.Services
{
	public class QuizesCreator(
		ILogger<AiContentCreatorBase<CreateQuizInput, IEnumerable<QuestionAiResponseDto>, Quiz>> logger,
		IAiQuestionsConverter aiQuestionConverter
			) : AiContentCreatorBase<CreateQuizInput, IEnumerable<QuestionAiResponseDto>, Quiz>(logger), IQuizesCreator
	{
		private readonly IAiQuestionsConverter _aiQuestionConverter = aiQuestionConverter;

		public async Task<IEnumerable<Question>?> GetQuizQuestionsAsync(string technologyName, AdvanceNumber advanceNumber, Guid quizId)
		{
			var body = await GenerateContentAsync(new CreateQuizInput(technologyName, advanceNumber, null));

			if (body == null) return null;

			try
			{
				var questions = JsonConvert.DeserializeObject<IEnumerable<QuestionAiResponseDto>>(body);

				var convertedQuestions = _aiQuestionConverter.ConvertToQuestions(questions!, quizId);

				return convertedQuestions;
			}
			catch (Exception ex)
			{
				_logger.LogError("Quizes creator - {ex}", ex);
				return null;
			}
		}

		protected override Quiz Convert(IEnumerable<QuestionAiResponseDto> output, CreateQuizInput input)
		{
			return _aiQuestionConverter.ConvertToQuiz(output, input.technologyName, input.advanceNumber, input.quizTitle);
		}

		protected override string GetPrompt(CreateQuizInput input)
		{
			return GptPrompts.CreateQuizPrompt(input.technologyName, input.advanceNumber.Number);
		}
	}
}
