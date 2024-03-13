using Application.Dto;
using Application.Services;
using Castle.Core.Logging;
using Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Application.Services
{
    public class AiQuestionToQuizConverterTests
    {
        private readonly Mock<ILogger<AiQuestionsConverter>> _loggerMock;
        private readonly AiQuestionsConverter _aiQuestionsToQuizConverter;

        public AiQuestionToQuizConverterTests()
        {
            _loggerMock = new();
            _aiQuestionsToQuizConverter = new(_loggerMock.Object);
        }

        [Fact]
        public void Convert_Should_AutomaticlyCreateQuizTitleString_WhenTitleNotProvided()
        {
            IEnumerable<QuestionAiResponseDto> response = [];
            var advanceNumber = AdvanceNumber.Create(5)!;

            var quiz = _aiQuestionsToQuizConverter.Convert(response, "techName", advanceNumber, null);

            quiz.Title
                .Should()
                .NotBeNull();

            quiz.Title
                .Should()
                .BeOfType<string>();
        }

        [Fact]
        public void Convert_ShouldNot_AutomaticlyCreateQuizTitleString_WhenTitleProvided()
        {
            IEnumerable<QuestionAiResponseDto> response = [];
            var advanceNumber = AdvanceNumber.Create(5)!;
            string title = "Fake title";

            var quiz = _aiQuestionsToQuizConverter.Convert(response, "techName", advanceNumber, title);

            quiz.Title
                .Should()
                .NotBeNull();

            quiz.Title
                .Should()
                .Be(title);
        }

        [Fact]
        public void Convert_Should_ReturnNoQuestions_WhenNoProperAnswerLetterInQuestionsRecognized()
        {
            IEnumerable<QuestionAiResponseDto> response = new List<QuestionAiResponseDto>()
            {
                new() { QuestionContent = "a. Question 1" , CharOfProperAnswer = 'x' } ,
                new() { QuestionContent = "b. Question 2" , CharOfProperAnswer = 'z'}
            };

            var advanceNumer = AdvanceNumber.Create(5)!;

            var quiz = _aiQuestionsToQuizConverter.Convert(response, "techName", advanceNumer, null);

            quiz.Questions.Count
                .Should()
                .Be(0);
        }

        [Fact]
        public void Convert_Should_ProperlyMapQuizQuestionsToHaveQuizIdIncluded()
        {
            IEnumerable<QuestionAiResponseDto> response = new List<QuestionAiResponseDto>()
            {
                new() { QuestionContent = "Question 1" , CharOfProperAnswer = 'b' } ,
                new() { QuestionContent = "Question 2" , CharOfProperAnswer = 'c'}
            };

            var advanceNumer = AdvanceNumber.Create(5)!;

            var quiz = _aiQuestionsToQuizConverter.Convert(response, "techName", advanceNumer, null);

            quiz.Id
                .Should()
                .Be((Guid)quiz.Questions[0].QuizId!);

            quiz.Id
                .Should()
                .Be((Guid)quiz.Questions[1].QuizId!);
        }

        [Fact]
        public void Convert_Should_ProperlyMapAnswersToHaveQuestionIdIncluded()
        {
            IEnumerable<string> answers = new List<string>()
            {
                "a. answer1" , "b. answer2"
            };

            IEnumerable<QuestionAiResponseDto> response = new List<QuestionAiResponseDto>()
            {
                new() { QuestionContent = "Question 1" , CharOfProperAnswer = 'b' , Answers = answers}
            };

            var advanceNumer = AdvanceNumber.Create(5)!;

            var quiz = _aiQuestionsToQuizConverter.Convert(response, "techName", advanceNumer, null);

            quiz.Questions[0].Id
                .Should()
                .Be(quiz.Questions[0].Answers[0].QuestionId);

            quiz.Questions[0].Id
                .Should()
                .Be(quiz.Questions[0].Answers[1].QuestionId);
        }

        [Fact]
        public void Convert_Should_ProperlyMapAnswersToNotStartWithAnswerCharLetter()
        {
            IEnumerable<string> answers = new List<string>()
            {
                "a. answer1" , "b. answer2"
            };

            IEnumerable<QuestionAiResponseDto> response = new List<QuestionAiResponseDto>()
            {
                new() { QuestionContent = "Question 1" , CharOfProperAnswer = 'b' , Answers = answers}
            };

            var advanceNumer = AdvanceNumber.Create(5)!;

            var quiz = _aiQuestionsToQuizConverter.Convert(response, "techName", advanceNumer, null);

            quiz.Questions[0].Answers[0].Content
                .Should()
                .NotStartWith("a.");

            quiz.Questions[0].Answers[1].Content
                .Should()
                .NotStartWith("b.");
        }
    }
}
