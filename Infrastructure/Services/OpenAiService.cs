using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OpenAiService : IAiService
    {
        private readonly ILogger<OpenAiService> _logger;

        public OpenAiService(ILogger<OpenAiService> logger)
        {
            _logger = logger;
        }

        public async Task<string?> GenerateContentAsync(string prompt)
        {
            try
            {
                var openAiService = new OpenAIService(new OpenAiOptions
                {
                    ApiKey = Environment.GetEnvironmentVariable("OpenAiApiKey")!,
                    DefaultModelId = Models.Gpt_3_5_Turbo
                });

                var completionResult = await openAiService.ChatCompletion.CreateCompletion(
                    new ChatCompletionCreateRequest
                    {
                        Messages = new List<ChatMessage>
                        {
                        ChatMessage.FromSystem(prompt)
                        }
                    });

                // retry (z Twojego kodu)
                if (!completionResult.Successful)
                {
                    await Task.Delay(20000);

                    completionResult = await openAiService.ChatCompletion.CreateCompletion(
                        new ChatCompletionCreateRequest
                        {
                            Messages = new List<ChatMessage>
                            {
                            ChatMessage.FromSystem(prompt)
                            }
                        });
                }

                if (!completionResult.Successful)
                {
                    _logger.LogWarning(
                        "AiService - AI response not successful: {msg}",
                        completionResult.Error?.Message ?? "Unknown"
                    );

                    return null;
                }

                var body = completionResult.Choices?.FirstOrDefault()?.Message?.Content;

                if (body == null)
                {
                    _logger.LogWarning("AiService - body of AI response is null!");
                    return null;
                }

                return body;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AiService error");
                return null;
            }
        }
    }
}
