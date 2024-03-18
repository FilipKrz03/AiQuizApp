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
		ILogger<AiContentCreatorBase<CreateAlgorithmInput, AlgorithmAiResponseDto, AlgorithmTask>> logger,
		IAiAlgorithmsConverter aiAlgorithmsConverter
		) : AiContentCreatorBase<CreateAlgorithmInput, AlgorithmAiResponseDto, AlgorithmTask>(logger), IAlgorithmsCreator
	{
		private readonly IAiAlgorithmsConverter _aiAlgorithmsConverter = aiAlgorithmsConverter;

		public async Task<(string, List<AlgorithmAnswer>)?>
			CreateAlgorithmContentAndAnswersAsync(AdvanceNumber advanceNumber, string specialTopics, Guid algorithmId)
		{
			var body = await GenerateContentAsync(new CreateAlgorithmInput(advanceNumber , "" ,  specialTopics));

			if (body == null) return null;

			try
			{
				var algorithm = JsonConvert.DeserializeObject<AlgorithmAiResponseDto>(body);

				return _aiAlgorithmsConverter.ConvertToAlgorithmContentAndAnswers(algorithm!, algorithmId);
			}
			catch (Exception ex)
			{
				_logger.LogError("ex - {ex}", ex);

				return null;
			}
		}

		protected override string GetPrompt(CreateAlgorithmInput input)
		{
			return GptPrompts.CreateAlgorithmPrompt(input.advanceNumber.Number, input.specialTopics);
		}

		protected override AlgorithmTask Convert(AlgorithmAiResponseDto output, CreateAlgorithmInput input)
		{
			return _aiAlgorithmsConverter.ConvertToAlgorithmTask(output, input.taskTitle, input.advanceNumber, input.specialTopics);
		}
	}
}
