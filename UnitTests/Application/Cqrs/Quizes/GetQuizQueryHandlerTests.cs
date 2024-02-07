using Application.Cqrs.Quiz.Query.GetQuiz;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.Interfaces;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Common;

namespace UnitTests.Application.Cqrs.Quizes
{
    public class GetQuizQueryHandlerTests
    {
        private readonly Mock<IRepository<Quiz>> _quizRepsitoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetQuizQueryHandlerTests()
        {
            _quizRepsitoryMock = new();
            _mapperMock = new();
        }

        [Fact]
        public async Task Handler_Should_ReturnQuizDetailResponseDtoObject_WhenQuizExist()
        {
            var request = new GetQuizQuery(Guid.NewGuid());

            string quizTitle = "simpleTitle";
            string technologyName = "simpleName";

            var quizes = new List<Quiz>()
            {
                new Quiz(Guid.NewGuid(), quizTitle, technologyName, AdvanceNumber.Create(5)!)
            };

            var quizQueryable = quizes.BuildMock();

            _quizRepsitoryMock.Setup(x => x.GetByIdQuery(It.IsAny<Guid>()))
                    .Returns(quizQueryable);

            _mapperMock.Setup(x => x.Map<QuizDetailResponseDto>(It.IsAny<Quiz>()))
                .Returns(new QuizDetailResponseDto()
                {
                    Title = quizTitle,
                    TechnologyName = technologyName,
                });

            GetQuizQueryHandler handler = new(_quizRepsitoryMock.Object, _mapperMock.Object);

            var result = await handler.Handle(request, default);

            result
                .Should()
                .BeOfType<QuizDetailResponseDto>();

            result.Title
                .Should()
                .Be(quizTitle);

            result.TechnologyName
                .Should()
                .Be(technologyName);
        }

        [Fact]
        public async Task Hadnler_Should_ThrowResourceNotFoundException_WhenQuizDoNotExist()
        {
            var request = new GetQuizQuery(Guid.NewGuid());

            IEnumerable<Quiz> quizes = [];

            var emptyQuizQueryable = quizes.BuildMock();

            _quizRepsitoryMock.Setup(x => x.GetByIdQuery(It.IsAny<Guid>()))
                    .Returns(emptyQuizQueryable);

            GetQuizQueryHandler handler = new(_quizRepsitoryMock.Object, _mapperMock.Object);

            await handler.Invoking(x => x.Handle(request, default))
                .Should()
                .ThrowAsync<ResourceNotFoundException>();
        }
    }
}
