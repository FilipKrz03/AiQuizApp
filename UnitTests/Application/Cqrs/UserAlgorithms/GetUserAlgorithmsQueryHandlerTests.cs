using Application.Common;
using Application.Cqrs.Algorithm.Query.GetAlgorithms;
using Application.Cqrs.UserAlgorithm.Query.GetAlgorithms;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Interfaces;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Application.Cqrs.UserAlgorithms
{
	public class GetUserAlgorithmsQueryHandlerTests
	{
		private readonly Mock<IRepository<UserOwnAlgorithmTask>> _userOwnAlgorithmTaskRepositoryMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly Mock<IUserRepository> _userRepositoryMock;

		private readonly GetUserAlgorithmsQueryHandler _handler;
        public GetUserAlgorithmsQueryHandlerTests()
        {
			_userOwnAlgorithmTaskRepositoryMock = new();
			_mapperMock = new();
			_userRepositoryMock = new();

			_handler = new(_userOwnAlgorithmTaskRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object);
        }

		[Fact]
		public async Task Handler_Should_ThrowInvalidTokenClaimException_WhenUserWithIdFromTokenClaimDoNotExist()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(false);

			await _handler.Invoking(x => x.Handle(new GetUserAlgorithmsQuery("" , new ResourceParamethersWithCreationStatus()), default))
				.Should()
				.ThrowAsync<InvalidTokenClaimException>();
		}

		[Fact]
		public async Task Handler_Should_Return_ProperlySortedPagedListOfUserOwnAlgortihmBasicResponseDto()
		{
			string userId = "testId";

			List<UserOwnAlgorithmTask> resultsFromRepo =
			[
				new(Guid.NewGuid() , "" , "" , "" , AdvanceNumber.Create(5)! , userId) ,
				new(Guid.NewGuid() , "" , "" , "" , AdvanceNumber.Create(7)!, userId) ,
			];

			List<UserOwnAlgorithmTaskBasicResponseDto> mappedResults =
			[
				new() {TaskTitle = "Title1"} ,
				new(){TaskTitle = "Title2"}
			];

			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			_userOwnAlgorithmTaskRepositoryMock.Setup(x => x.Query())
				.Returns(resultsFromRepo.BuildMock());

			_mapperMock.Setup(x => x.Map<PagedList<UserOwnAlgorithmTaskBasicResponseDto>>(It.IsAny<PagedList<UserOwnAlgorithmTask>>()))
				.Returns(new PagedList<UserOwnAlgorithmTaskBasicResponseDto>(mappedResults, 1, 1, 1));

			var result = await _handler.Handle(new GetUserAlgorithmsQuery(userId, new ResourceParamethersWithCreationStatus()) , default);

			result
				.Should()
				.NotBeNull();

			result
				.Should()
				.BeOfType<PagedList<UserOwnAlgorithmTaskBasicResponseDto>>();

			result.Count()
				.Should()
				.Be(2);

			result[0].TaskTitle
				.Should()
				.Be("Title1");

			result[1].TaskTitle
				.Should()
				.Be("Title2");
		}

	}
}
