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
using Domain.Interfaces;

namespace Application.Services
{
	public abstract class AiContentCreatorBase<TInput, TOutput, TConvertedOutput>
	{
		private readonly IAiService _aiService;
		protected readonly ILogger<AiContentCreatorBase<TInput, TOutput, TConvertedOutput>> _logger;

		protected AiContentCreatorBase(ILogger<AiContentCreatorBase<TInput, TOutput, TConvertedOutput>> logger, IAiService aiService)
		{
			_logger = logger;
			_aiService = aiService;
		}

		public async Task<TConvertedOutput?> CreateAsync(TInput inputData)
		{
			try
			{
				var body = await GenerateContentAsync(inputData);

				if (body == null) return default;

				var result = JsonConvert.DeserializeObject<TOutput>(body);

				var convertedResult = Convert(result!, inputData);
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
				var aiResponse = await _aiService.GenerateContentAsync(GetPrompt(input));
              
				if (aiResponse == null)
				{
					_logger.LogWarning("Content creator - body of Ai response is null !");
					return null;
				}

				return aiResponse;
			}
			catch (Exception ex)
			{
				_logger.LogError("Ai content creator base {ex}", ex);

				return null;
			}
		}

		protected abstract string GetPrompt(TInput input);

		protected abstract TConvertedOutput Convert(TOutput output, TInput input);
	}
}
