using Application.Cqrs.Algorithm.Query.GetAlgorithm;
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

namespace UnitTests.Application.Cqrs.Algorithms
{
	public class GetAlgorithmQueryHandlerTests
	{
		private readonly Mock<IRepository<AlgorithmTask>> _algoithmTaskRepositroyMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly GetAlgorithmQueryHandler _handler;
		public GetAlgorithmQueryHandlerTests()
		{
			_algoithmTaskRepositroyMock = new();
			_mapperMock = new();

			_handler = new(_algoithmTaskRepositroyMock.Object, _mapperMock.Object);
		}

		[Fact]
		public async Task Handler_Should_ThrowResourceNotFoundException_WhenAlgorithmDoNotExist()
		{
			IEnumerable<AlgorithmTask> emptyAlgorithmTaskEnum = [];

			_algoithmTaskRepositroyMock.Setup(x =>
				x.GetByIdQuery(It.IsAny<Guid>())).Returns(emptyAlgorithmTaskEnum.BuildMock());

			await _handler.Invoking(x => x.Handle(new GetAlgorithmQuery(Guid.NewGuid()), default))
				.Should()
				.ThrowAsync<ResourceNotFoundException>();
		}

		[Fact]
		public async Task Handler_Should_ReturnMappedAlgorithmDetailResponse_WhenAlgoirthmFound()
		{
			List<AlgorithmTask> algorithmTaskList = new()
			{
				new(Guid.NewGuid() , "" , "" , "" , AdvanceNumber.Create(1)!)
			};

			_algoithmTaskRepositroyMock.Setup(x =>
				x.GetByIdQuery(It.IsAny<Guid>())).Returns(algorithmTaskList.BuildMock());

			_mapperMock.Setup(x => x.Map<AlgorithmTaskDetailResponseDto>(It.IsAny<AlgorithmTask>()))
				.Returns(new AlgorithmTaskDetailResponseDto()
				{
					TaskTitle = "Test"
				});

			var result = await _handler.Handle(new GetAlgorithmQuery(Guid.NewGuid()), default);

			result
				.Should()
				.NotBeNull();

			result
				.Should()
				.BeOfType<AlgorithmTaskDetailResponseDto>();

			result.TaskTitle
				.Should()
				.Be("Test");
		}
	}
}
