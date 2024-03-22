using Application.Cqrs.UserAlgorithm.Query.GetAlgorithm;
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

namespace UnitTests.Application.Cqrs.UserAlgorithms
{
	public class GetUserAlgorithmQueryHandlerTests
	{
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IRepository<UserOwnAlgorithmTask>> _userOwnAlgorithmTaskRepository;
		private readonly Mock<IMapper> _mapperMock;

		private readonly GetUserAlgorithmQueryHandler _handler;

		public GetUserAlgorithmQueryHandlerTests()
		{
			_userRepositoryMock = new();
			_userOwnAlgorithmTaskRepository = new();
			_mapperMock = new();

			_handler = new(_userRepositoryMock.Object, _userOwnAlgorithmTaskRepository.Object, _mapperMock.Object);
		}

		[Fact]
		public async Task Handler_Should_ThrowInvalidTokenClaimException_WhenUserWithIdFromTokenClaimDoNotExist()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(false);

			await _handler.Invoking(x => x.Handle(new GetUserAlgorithmQuery("", Guid.NewGuid()), default))
				.Should()
				.ThrowAsync<InvalidTokenClaimException>();
		}

		[Fact]
		public async Task Handler_Should_ThrowResourceNotFoundException_WhenAlgorithmNotFound()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
			.ReturnsAsync(true);

			_userOwnAlgorithmTaskRepository.Setup(x => x.GetByIdQuery(It.IsAny<Guid>()))
				.Returns(new List<UserOwnAlgorithmTask>().BuildMock());

			await _handler.Invoking(x => x.Handle(new GetUserAlgorithmQuery("", Guid.NewGuid()), default))
				.Should()
				.ThrowAsync<ResourceNotFoundException>();
		}

		[Fact]
		public async Task Handler_Should_ReturnProperlyMappedAlgorithm_WhenAlgorithmFound()
		{
			Guid searchedAlgorithmId = Guid.NewGuid();
			string userId = "fakeId";

			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			_userOwnAlgorithmTaskRepository.Setup(x => x.GetByIdQuery(It.IsAny<Guid>()))
				.Returns(new List<UserOwnAlgorithmTask>()
				{
					new(searchedAlgorithmId , "SimpleTaskTitle" , "" ,"" ,AdvanceNumber.Create(1)! , userId)
				}.BuildMock());

			_mapperMock.Setup(x => x.Map<AlgorithmTaskDetailResponseDto>(It.IsAny<UserOwnAlgorithmTask>()))
				.Returns(new AlgorithmTaskDetailResponseDto()
				{
					TaskTitle = "SimpleTaskTitle"
				});

			var result = await _handler.Handle(new GetUserAlgorithmQuery(userId, searchedAlgorithmId) , default!);

			result
				.Should()
				.NotBeNull();

			result
				.Should()
				.BeOfType<AlgorithmTaskDetailResponseDto>();

			result.TaskTitle
				.Should()
				.Be("SimpleTaskTitle");
		}
	}
}
