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

namespace Application.Services
{
    public class QuizesCreator : IQuizesCreator
    {
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
        }
    }
}
