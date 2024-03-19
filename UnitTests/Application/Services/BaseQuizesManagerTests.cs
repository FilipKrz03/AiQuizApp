using Application.Dto;
using Application.Interfaces;
using Application.Props;
using Application.Services;
using Castle.Core.Logging;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace UnitTests.Application.Services
{
    public class BaseQuizesManagerTests
    {
        private readonly Mock<ILogger<BaseQuizesManager>> _loggerMock;
        private readonly Mock<IRepository<Quiz>> _quizRepositoryMock;
        private readonly Mock<IQuizesCreator> _quizesCreatorMock;

        private readonly BaseQuizesManager _baseQuizesManager;

        public BaseQuizesManagerTests()
        {
            // Base setup before each test

            _loggerMock = new();

            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var quizRepositoryMock = new Mock<IRepository<Quiz>>();
            var quizesCreatorMock = new Mock<IQuizesCreator>();

            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            serviceScopeFactoryMock
                .Setup(x => x.CreateScope())
                .Returns(serviceScopeMock.Object);

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactoryMock.Object);

            serviceScopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);

            serviceProviderMock.Setup(x => x.GetService(typeof(IRepository<Quiz>)))
                .Returns(quizRepositoryMock.Object);

            serviceProviderMock.Setup(x => x.GetService(typeof(IQuizesCreator)))
                .Returns(quizesCreatorMock.Object);

            _quizesCreatorMock = quizesCreatorMock;
            _quizRepositoryMock = quizRepositoryMock;

            _baseQuizesManager = new(_loggerMock.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Manager_QuizesCreator_Should_CallProperTimesCountDependingOfQuizesWithBaseTechnologiesExisting()
        {
            var advanceNumber = AdvanceNumber.Create(5)!;

            IEnumerable<Quiz> quizes = [];

            var expectedTimesCall = BaseTechnologies.Get().Count();

            _quizRepositoryMock.Setup(x => x.Query())
                 .Returns(quizes.BuildMock());

            await _baseQuizesManager.StartAsync(default);

            _quizesCreatorMock.Verify
               (x => x.CreateAsync(
                   It.IsAny<CreateQuizInput>()
                   ),
                   Times.Exactly(expectedTimesCall)
                  );
        }
    }
}
