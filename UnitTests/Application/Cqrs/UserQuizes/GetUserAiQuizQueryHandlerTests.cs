using Application.Cqrs.UserQuiz.Query.GetUserAiQuizQuery;
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

namespace UnitTests.Application.Cqrs.UserQuizes
{
	public class GetUserAiQuizQueryHandlerTests
	{
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IRepository<UserOwnQuiz>> _userOwnQuizRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly GetUserAiQuizQueryHandler _handler;

        public GetUserAiQuizQueryHandlerTests()
        {
			_userRepositoryMock = new();
			_userOwnQuizRepositoryMock = new();
			_mapperMock = new();

			_handler = new(_userOwnQuizRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }


		[Fact]
		public async Task Handler_Should_ThrowInvalidAccesTokenException_WhenUserWithIdFromTokenNotFound()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(false);

			await _handler.Invoking(x => x.Handle(new GetUserAiQuizQuery(Guid.NewGuid(), ""), default!))
				.Should()
				.ThrowAsync<InvalidTokenClaimException>();
		}

		[Fact]
		public async Task Handler_Should_ReturnQuizDetailResponse_WhenUserQuizFound()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			IEnumerable<UserOwnQuiz> enumWithUserQuiz = new List<UserOwnQuiz>()
			{
				new UserOwnQuiz(Guid.NewGuid(), "", "", AdvanceNumber.Create(1)!)
			};

			QuizDetailResponseDto mappedQuiz = new()
			{
				Title = "SimpleTitle",
				TechnologyName = "SimpleTechnology",
				AdvanceNumber = 5
			};

			_userOwnQuizRepositoryMock.Setup(x => x.GetByIdQuery(It.IsAny<Guid>()))
				.Returns(enumWithUserQuiz.BuildMock());

			_mapperMock.Setup(x => x.Map<QuizDetailResponseDto>(It.IsAny<UserOwnQuiz>()))
				.Returns(mappedQuiz);

			var result = await _handler.Handle(new GetUserAiQuizQuery(Guid.NewGuid(), ""), default!);

			result
				.Should()
				.NotBeNull();

			result
				.Should()
				.BeAssignableTo<QuizDetailResponseDto>();

			result.Title
				.Should()
				.Be("SimpleTitle");

			result.TechnologyName
				.Should()
				.Be("SimpleTechnology");

			result.AdvanceNumber
				.Should()
				.Be(5);
		}
    }
}
