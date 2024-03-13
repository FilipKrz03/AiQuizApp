using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public sealed class AiQuestionsConverter
        (ILogger<AiQuestionsConverter> logger) : IAiQuestionsConverter
    {
        private readonly ILogger<AiQuestionsConverter> _logger = logger;

        public Quiz ConvertToQuiz
            (IEnumerable<QuestionAiResponseDto> response, string technologyName, AdvanceNumber advanceNumber , string? quizTitle)
        {
            var quizId = Guid.NewGuid();

            Quiz quiz = new(quizId, quizTitle, technologyName, advanceNumber)
            {
                Questions = GetQuestions(response, quizId).ToList()
            };

            return quiz;
        }

        public List<Question> ConvertToQuestions(IEnumerable<QuestionAiResponseDto> response, Guid quizId)
        {
            return GetQuestions(response, quizId).ToList(); 
        }

        private IEnumerable<Question> GetQuestions(IEnumerable<QuestionAiResponseDto> questions, Guid quizId)
        {
            foreach (var question in questions)
            {
                var properAnswerLetter = AnswerLetter.Create(question.CharOfProperAnswer);

                if (properAnswerLetter != null)
                {
                    var questionId = Guid.NewGuid();

                    yield return new Question(questionId, question.QuestionContent, properAnswerLetter)
                    {
                        QuizId = quizId,
                        Answers = GetAnswers(question.Answers, questionId).ToList()
                    };
                }
                else
                {
                    _logger.LogWarning("AiQuestionsConverter - not recognized proper answer letter");
                }
            }
        }

        private IEnumerable<Answer> GetAnswers(IEnumerable<string> answers, Guid questionId)
        {
            foreach (var answer in answers)
            {
                var answerSplit = answer.Split(".");

                char? answerLetter = GetAnswerLetter(answerSplit[0]);

                if (answerLetter != null)
                {
                    var answerLetterObject = AnswerLetter.Create((char)answerLetter);

                    yield return new Answer(Guid.NewGuid(), answerSplit[1], answerLetterObject!)
                    {
                        QuestionId = questionId,
                    };
                }
                else
                {
                    _logger.LogWarning("AiQuestionsConverter - Cannot recognize answer letter : {answer}", answer);
                }
            }
        }

        private char? GetAnswerLetter(string answerString) => answerString switch
        {
            _ when answerString.Contains('a') => 'a',
            _ when answerString.Contains('b') => 'b',
            _ when answerString.Contains('c') => 'c',
            _ when answerString.Contains('d') => 'd',
            _ => null
        };
    }
}
