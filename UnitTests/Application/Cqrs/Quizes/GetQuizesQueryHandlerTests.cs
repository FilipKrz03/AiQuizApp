using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Cqrs.Quiz.Query.GetQuizes;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.Interfaces;
using MockQueryable.Moq;
using Moq;

namespace UnitTests.Application.Cqrs.Quizes
{
    public sealed class GetQuizesQueryHandlerTests
    {
        private readonly Mock<IRepository<Quiz>> _quizRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetQuizesQueryHandlerTests()
        {
            _quizRepositoryMock = new();
            _mapperMock = new();
        }

        [Fact]
        public async Task Handler_Should_ReturnEmptyPagedList_WhenNoQuizes()
        {
            var request = new GetQuizesQuery(new ResourceParamethers());

            IEnumerable<Quiz> emptyQuizes = [];

            List<QuizBasicResponseDto> emptyBasicResposne = [];

            var pagedList = new PagedList<QuizBasicResponseDto>(emptyBasicResposne, 1, 1, 1);

            _quizRepositoryMock.Setup(x => x.Query())
                .Returns(emptyQuizes.BuildMock());

            _mapperMock.Setup(x => x.Map<PagedList<QuizBasicResponseDto>>(It.IsAny<PagedList<Quiz>>()))
                .Returns(pagedList);

            var handler = new GetQuizesQueryHandler(_quizRepositoryMock.Object, _mapperMock.Object);

            var result = await handler.Handle(request, default);

            result.Count
                .Should()
                .Be(0);

            result
                .Should()
                .BeOfType<PagedList<QuizBasicResponseDto>>();
        }

        [Fact]
        public async Task Handler_Should_ProperlyHandledPagedList_WhenSpecificResourceParamethersGiven()
        {
            var request = new GetQuizesQuery(new ResourceParamethers() { PageSize = 1});

            List<Quiz> quizes = new()
            {
                new(Guid.NewGuid() , "" , "", AdvanceNumber.Create(5)!) ,
                new(Guid.NewGuid() , "" , "", AdvanceNumber.Create(5)!)
            };

            List<QuizBasicResponseDto> basicResposnes = new()
            {
                new QuizBasicResponseDto()
            };

            var pagedList = new PagedList<QuizBasicResponseDto>(basicResposnes, 1, 1, 1);

            _quizRepositoryMock.Setup(x => x.Query())
                .Returns(quizes.BuildMock());

            _mapperMock.Setup(x => x.Map<PagedList<QuizBasicResponseDto>>(It.IsAny<PagedList<Quiz>>()))
                .Returns(pagedList);

            var handler = new GetQuizesQueryHandler(_quizRepositoryMock.Object, _mapperMock.Object);

            var result = await handler.Handle(request, default);

            result.Count
                .Should()
                .Be(1);

            result
                .Should()
                .BeOfType<PagedList<QuizBasicResponseDto>>();
        }
    }
}
