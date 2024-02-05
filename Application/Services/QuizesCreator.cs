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

namespace Application.Services
{
    public class QuizesCreator : IQuizesCreator
    {
        private readonly ILogger<QuizesCreator> _logger;

        public QuizesCreator(ILogger<QuizesCreator> logger)
        {
            _logger = logger;
        }

        public async Task Create(string technologyName, AdvanceNumber advanceNumber)
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

            if(body == null)
            {
                _logger.LogWarning("Quizes creator - body of Ai response is null !");
                return;
            }

            try
            {
                var questions = JsonConvert.DeserializeObject<IEnumerable<QuestionAiResponseDto>>(body);

                // Todo
            }
            catch(Exception ex)
            {
                _logger.LogError("Quizes creator - {ex}", ex);
            }
        }
    }
}
