using Application.Props;
using Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;
using OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public abstract class AiContentCreatorBase<TInput , TOutput , TConvertedOutput>
	{
		protected readonly ILogger<AiContentCreatorBase<TInput, TOutput, TConvertedOutput>> _logger;	

		protected AiContentCreatorBase(ILogger<AiContentCreatorBase<TInput, TOutput, TConvertedOutput>> logger)
		{
			_logger = logger;
		}

		public async Task <TConvertedOutput?> CreateAsync(TInput inputData)
		{
			var body = await GenerateContentAsync(inputData);

			if (body == null) return default;

			try
			{
				var result = JsonConvert.DeserializeObject<TOutput>(body);

				var convertedResult = Convert(result! , inputData);
				return convertedResult;
			}
			catch (Exception ex)
			{
				_logger.LogError("Content creation error - {ex}", ex);
				return default;
			}
		}

		protected async Task<string?> GenerateContentAsync(TInput input)
		{
			try
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
						ChatMessage.FromSystem(GetPrompt(input))
						}
					});

				var body = completionResult?.Choices?.FirstOrDefault()?.Message?.Content;

				if (body == null)
				{
					_logger.LogWarning("Content creator - body of Ai response is null !");
				}

				return body!;
			}
			catch(Exception ex)
			{
				_logger.LogError("Ai content creator base {ex}", ex);

				return null;
			}
		}

		protected abstract string GetPrompt(TInput input);
		
		protected abstract TConvertedOutput Convert(TOutput output, TInput input);
	}
}
