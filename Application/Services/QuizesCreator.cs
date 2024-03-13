﻿using Domain.ValueObjects;
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
		ILogger<QuizesCreator> logger,
		IAiQuestionsConverter aiQuestionConverter
			) : IQuizesCreator
	{
		private readonly ILogger<QuizesCreator> _logger = logger;
		private readonly IAiQuestionsConverter _aiQuestionConverter = aiQuestionConverter;

		public async Task<Quiz?> CreateAsync(string technologyName, AdvanceNumber advanceNumber, string? quizTitle)
		{
			var body = await CreateQuizQuestionsByAiAsync(technologyName, advanceNumber);

			if (body == null) return null;
	
			try
			{
				var questions = JsonConvert.DeserializeObject<IEnumerable<QuestionAiResponseDto>>(body);

				var quiz = _aiQuestionConverter.ConvertToQuiz(questions!, technologyName, advanceNumber, quizTitle);

				return quiz;
			}
			catch (Exception ex)
			{
				_logger.LogError("Quizes creator - {ex}", ex);
				return null;
			}
		}

		public async Task<IEnumerable<Question>?> GetQuizQuestionsAsync(string technologyName, AdvanceNumber advanceNumber, Guid quizId)
		{
			var body = await CreateQuizQuestionsByAiAsync(technologyName, advanceNumber);

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

		private async Task<string?> CreateQuizQuestionsByAiAsync(string technologyName, AdvanceNumber advanceNumber)
		{
			var openAiService = new OpenAIService(new OpenAiOptions()
			{
				ApiKey = Environment.GetEnvironmentVariable("OpenAiApiKey")!,
				DefaultModelId = Models.Gpt_3_5_Turbo
			});

			var completionResult =
				await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
				{
					Messages = new List<ChatMessage>
					{
						ChatMessage.FromSystem(GptPrompts.CreateQuizPrompt(technologyName , advanceNumber.Number))
					}
				});

			var body = completionResult.Choices.First().Message.Content;

			if (body == null)
			{
				_logger.LogWarning("Quizes creator - body of Ai response is null !");
			}

			return body;
		}
	}
}
