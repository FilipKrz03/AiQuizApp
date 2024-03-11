using Application.Cqrs.UserQuiz.Command.CreateAiQuiz;
using Domain.Exceptions;
using FluentAssertions;
using Infrastructure.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Application.Cqrs.UserQuizes
{
	public class CreateAiQuizCommandHandlerTests
	{
		private readonly Mock<IServiceProvider> _serviceProviderMock;
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly CreateAiQuizCommandHandler _handler;

        public CreateAiQuizCommandHandlerTests()
        {
			_serviceProviderMock = new();
			_userRepositoryMock = new();

			_handler = new(_serviceProviderMock.Object, _userRepositoryMock.Object);
        }

		[Fact]
		public async Task Handler_Should_ThrowInvalidTokenClaimException_WhenUserWithIdFromTokenClaimNotFound()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
			   .ReturnsAsync(false);

			await _handler.Invoking(x => x.Handle(new CreateAiQuizCommand("" , "" , 5 , ""), default!))
				.Should()
				.ThrowAsync<InvalidTokenClaimException>();
		}

		[Fact]	
		public async Task Handler_Should_ReturnProperStringInformationWhenUserExist()
		{
			_userRepositoryMock.Setup(x => x.UserExistAsync(It.IsAny<string>()))
			   .ReturnsAsync(true);

			var result = await _handler.Handle(new CreateAiQuizCommand("", "", 5, ""), default!);

			result
				.Should()
				.NotBeNull();

			result
				.Should()
				.BeOfType<string>();
		}	
    }
}
