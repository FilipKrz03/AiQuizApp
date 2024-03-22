using Application.Cqrs.UserAlgorithm.Command.CreateAlgorithm;
using Domain.Exceptions;
using FluentAssertions;
using Infrastructure.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Application.Cqrs.UserAlgorithms
{
	public class CreateUserAlgorithmCommandHandlerTests
	{
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IServiceProvider> _serviceProviderMock;

        private readonly CreateAlgorithmCommandHandler _handler;
        public CreateUserAlgorithmCommandHandlerTests()
        {
            _userRepositoryMock = new();
            _serviceProviderMock = new();

            _handler = new(_userRepositoryMock.Object, _serviceProviderMock.Object);
        }

		[Fact]
		public async Task Handler_Should_ThrowInvalidTokenClaimException_WhenUserWithIdFromTokenClaimDoNotExist()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(false);

			await _handler.Invoking(x => x.Handle(new CreateAlgorithmCommand("" , "" , "" ,  5), default))
				.Should()
				.ThrowAsync<InvalidTokenClaimException>();
		}

		[Fact]
		public async Task Hanlder_Should_ReturnCreationConfrimationString_WhenUserFound()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
				.ReturnsAsync(true);

			var result = await _handler.Handle(new CreateAlgorithmCommand("", "", "", 5), default);

			result
				.Should()
				.NotBeNull();

			result
				.Should()
				.BeOfType<string>();

			result
				.Should()
				.Be("Algorithm creation queued");
		}
	}
}
