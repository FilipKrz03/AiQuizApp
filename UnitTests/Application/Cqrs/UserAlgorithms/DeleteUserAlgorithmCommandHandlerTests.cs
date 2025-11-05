using Application.Cqrs.UserAlgorithm.Command.DeleteAlgorithm;
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
	public class DeleteUserAlgorithmCommandHandlerTests
	{
		private readonly Mock<IRepository<AlgorithmTask>> _algorithmTaskRepository;
		private readonly Mock<IUserRepository> _userRepositoryMock;

		private readonly DeleteUserAlgorithmCommandHandler _handler;

		public DeleteUserAlgorithmCommandHandlerTests()
		{
			_algorithmTaskRepository = new();
			_userRepositoryMock = new();

			_handler = new(_algorithmTaskRepository.Object, _userRepositoryMock.Object);
		}

		[Fact]
		public async Task Handler_Should_ThrowInvalidTokenClaimException_WhenUserWithIdFromTokenClaimDoNotExist()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(false);

			await _handler.Invoking(x => x.Handle(new DeleteUserAlgorithmCommand("", Guid.NewGuid()), default))
				.Should()
				.ThrowAsync<InvalidTokenClaimException>();
		}

		[Fact]
		public async Task Handler_Should_ThrowResourceAlreadyNotExistException_WhenAlgorithmNotFound()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			_algorithmTaskRepository.Setup(x => x.GetByIdQuery(It.IsAny<Guid>()))
				.Returns(new List<AlgorithmTask>().BuildMock());

			await _handler.Invoking(x => x.Handle(new DeleteUserAlgorithmCommand("", Guid.NewGuid()), default))
			.Should()
			.ThrowAsync<ResourceAlreadyNotExistException>();
		}

		[Fact]
		public async Task Handler_Should_CallDeleteEntityAndSaveChangesToRepository_WhenAlgorithmFound()
		{
			Guid algorthmId = Guid.NewGuid();
			string userId = "testId";

			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			_algorithmTaskRepository.Setup(x => x.GetByIdQuery(It.IsAny<Guid>()))
				.Returns(new List<AlgorithmTask>()
				{
					new(algorthmId  , "" , "" , "" , AdvanceNumber.Create(5)! , userId)
				}.BuildMock());

			await _handler.Handle(new DeleteUserAlgorithmCommand(userId, algorthmId), default);

			_algorithmTaskRepository.Verify(x => x.DeleteEntity(It.IsAny<AlgorithmTask>()), Times.Once);
			_algorithmTaskRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
		}
	}
}
