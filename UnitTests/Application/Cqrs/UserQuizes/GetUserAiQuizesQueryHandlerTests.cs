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
		private readonly Mock<IRepository<Quiz>> _userOwnQuizRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly GetUserQuizesQueryHandler _handler;

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

			await _handler.Invoking(x => x.Handle(new GetUserQuizesQuery("", new ResourceParamethersWithCreationStatus()), default!))
				.Should()
				.ThrowAsync<InvalidTokenClaimException>();
		}

		[Fact]
		public async Task Handler_Should_ReturnProperRecordsBaseOnSearchQuery()
		{
			Guid userId = Guid.NewGuid();

			List<Quiz> userOwnQuizzes = new()
			{
				new Quiz(Guid.NewGuid() , "Java 6" , "JavaFX" , AdvanceNumber.Create(5)!, userId.ToString()) ,
				new Quiz(Guid.NewGuid() , "Aaa" , "aaa" , AdvanceNumber.Create(5)!,userId.ToString()) ,
				new Quiz(Guid.NewGuid() , "Java library tests" , "Spring" , AdvanceNumber.Create(5)!, userId.ToString()),
			};

			List<UserOwnQuizBasicResponseDto> userOwnQuizzesResponse = new()
			{
				new UserOwnQuizBasicResponseDto()
				{
					Id = Guid.NewGuid(),
					Title = "Java 6"
				},
				new UserOwnQuizBasicResponseDto()
				{
					Id = Guid.NewGuid(),
					Title = "Java library tests"
				},
			};

			PagedList<UserOwnQuizBasicResponseDto> userOwnQuizesPagedList = new(userOwnQuizzesResponse, 1, 1, 1);

			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			_userOwnQuizRepositoryMock.Setup(x => x.Query())
				.Returns(userOwnQuizzes.BuildMock());

			_mapperMock.Setup(x => x.Map<PagedList<UserOwnQuizBasicResponseDto>>(It.IsAny<PagedList<Quiz>>()))
				.Returns(userOwnQuizesPagedList);	

			ResourceParamethersWithCreationStatus resourceParamethers = new()
			{
				SearchQuery = "Java"
			};

			var result = await _handler.Handle(new GetUserQuizesQuery(userId.ToString(), resourceParamethers), default!);

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
