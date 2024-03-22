using Application.Cqrs.UserQuiz.Command.DeleteAiQuiz;
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
	public class DeleteAiQuizCommandHandlerTests
	{
		private readonly Mock<IRepository<UserOwnQuiz>> _userOwnQuizRepositoryMock;
		private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DeleteUserQuizCommandHandler _handler;

        public DeleteAiQuizCommandHandlerTests()
        {
            _userOwnQuizRepositoryMock = new();
            _userRepositoryMock = new();

            _handler = new(_userOwnQuizRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handler_Should_ThrowInvalidTokenClaimException_When_UserWithIdFromTokenClaimDoNotExist()
        {
            _userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            await _handler.Invoking(x => x.Handle(new DeleteUserQuizCommand(Guid.NewGuid(), ""), default!))
                .Should()
                .ThrowAsync<InvalidTokenClaimException>();
        }

        [Fact]
        public async Task Handler_Should_ThrowResourceAlreadyNotExistException_When_QuizToDeleteNotFound()
        {
            IEnumerable<UserOwnQuiz> emptyUserOwnQuizEnum = new List<UserOwnQuiz>();

			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

            _userOwnQuizRepositoryMock.Setup(x => x.GetByIdQuery(It.IsAny<Guid>()))
                .Returns(emptyUserOwnQuizEnum.BuildMock());

			await _handler.Invoking(x => x.Handle(new DeleteUserQuizCommand(Guid.NewGuid(), ""), default!))
			   .Should()
			   .ThrowAsync<ResourceAlreadyNotExistException>();
		}

        [Fact]
        public async Task Handler_Should_CallDeleteAndSaveChangesToRepository_When_QuizToDeleteFound()
        {
            var fakeUserId = "Fake";

            IEnumerable<UserOwnQuiz> userOwnQuizEnum = new List<UserOwnQuiz>()
            {
                new UserOwnQuiz(Guid.NewGuid() , "" , "" , AdvanceNumber.Create(1)! , fakeUserId)
            };

			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			_userOwnQuizRepositoryMock.Setup(x => x.GetByIdQuery(It.IsAny<Guid>()))
				.Returns(userOwnQuizEnum.BuildMock());

            await _handler.Handle(new DeleteUserQuizCommand(Guid.NewGuid(), fakeUserId), default!);

            _userOwnQuizRepositoryMock.Verify(x => x.DeleteEntity(It.IsAny<UserOwnQuiz>()));
			_userOwnQuizRepositoryMock.Verify(x => x.SaveChangesAsync());
		}
    }
}
