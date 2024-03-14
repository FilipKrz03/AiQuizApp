using Application.Props;
using Domain.ValueObjects;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;
using OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Application.Dto;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
	public class AlgorithmsCreator(
		ILogger<AlgorithmsCreator> logger , 
		IAiAlgorithmsConverter aiAlgorithmsConverter
		) : IAlgorithmsCreator
	{
		private readonly ILogger<AlgorithmsCreator> _logger = logger;
		private readonly IAiAlgorithmsConverter _aiAlgorithmsConverter = aiAlgorithmsConverter;

		public async Task<AlgorithmTask?> CreateAsync(AdvanceNumber advanceNumber, string taskTitle , string specialTopics)
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
						ChatMessage.FromSystem(GptPrompts.CreateAlgorithmPrompt(advanceNumber.Number , specialTopics))
					}
				});

			var body = completionResult.Choices.First().Message.Content;

			if (body == null)
			{
				_logger.LogWarning("Algorithems creator - body of Ai response is null !");
				return null;
			}

			try
			{
				var algorithm = JsonConvert.DeserializeObject<AlgorithmAiResponseDto>(body);

				return _aiAlgorithmsConverter.ConvertToAlgorithmTask(algorithm! , taskTitle, advanceNumber , specialTopics);
			}
			catch (Exception ex)
			{
				_logger.LogError("ex - {ex}", ex);

				return null;
			}
		}
	}
}
