using Application.Common;
using Application.Cqrs.UserQuiz.Query.GetUserAiQuizesQuery;
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
	public class GetUserAiQuizesQueryHandlerTests
	{
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IRepository<UserOwnQuiz>> _userOwnQuizRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly GetUserAiQuizesQueryHandler _handler;

		public GetUserAiQuizesQueryHandlerTests()
		{
			_userRepositoryMock = new();
			_userOwnQuizRepositoryMock = new();
			_mapperMock = new();

			_handler = new(_userRepositoryMock.Object, _userOwnQuizRepositoryMock.Object, _mapperMock.Object);
		}

		[Fact]
		public async Task Handler_Should_ThrowInvalidTokenClaimException_WhenNoUserWithIdFromTokenClaimFound()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(false);

			await _handler.Invoking(x => x.Handle(new GetUserAiQuizesQuery("", new ResourceParamethersWithCreationStatus()), default!))
				.Should()
				.ThrowAsync<InvalidTokenClaimException>();
		}

		[Fact]
		public async Task Handler_Should_ReturnProperRecordsBaseOnSearchQuery()
		{
			List<UserOwnQuiz> userOwnQuizzes = new()
			{
				new UserOwnQuiz(Guid.NewGuid() , "Java 6" , "JavaFX" , AdvanceNumber.Create(5)!) ,
				new UserOwnQuiz(Guid.NewGuid() , "Aaa" , "aaa" , AdvanceNumber.Create(5)!) ,
				new UserOwnQuiz(Guid.NewGuid() , "Java library tests" , "Spring" , AdvanceNumber.Create(5)!),
			};

			List<QuizBasicResponseDto> userOwnQuizzesResponse = new()
			{
				new QuizBasicResponseDto()
				{
					Id = Guid.NewGuid(),
					Title = "Java 6"
				},
				new QuizBasicResponseDto()
				{
					Id = Guid.NewGuid(),
					Title = "Java library tests"
				},
			};

			PagedList<QuizBasicResponseDto> userOwnQuizesPagedList = new(userOwnQuizzesResponse, 1, 1, 1);

			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			_userOwnQuizRepositoryMock.Setup(x => x.Query())
				.Returns(userOwnQuizzes.BuildMock());

			_mapperMock.Setup(x => x.Map<PagedList<QuizBasicResponseDto>>(It.IsAny<PagedList<UserOwnQuiz>>()))
				.Returns(userOwnQuizesPagedList);	

			ResourceParamethersWithCreationStatus resourceParamethers = new()
			{
				SearchQuery = "Java"
			};

			var result = await _handler.Handle(new GetUserAiQuizesQuery("", resourceParamethers), default!);

			result.Count
				.Should()
				.Be(2);

			result[0].Title
				.Should()
				.BeOneOf("Java 6", "Java library tests");

			result[1].Title
				.Should()
				.BeOneOf("Java", "Java library tests");
		}
	}
}
