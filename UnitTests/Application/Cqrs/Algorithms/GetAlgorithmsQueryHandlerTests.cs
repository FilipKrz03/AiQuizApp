using Application.Common;
using Application.Cqrs.Algorithm.Query.GetAlgorithms;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
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
	public class GetAlgorithmsQueryHandlerTests
	{
		private readonly Mock<IRepository<AlgorithmTask>> _algorithmTaskRepository;
		private readonly Mock<IMapper> _mapperMock;
		private readonly GetAlgorithmsQueryHandler _handler;
		public GetAlgorithmsQueryHandlerTests()
		{
			_algorithmTaskRepository = new();
			_mapperMock = new();

			_handler = new(_algorithmTaskRepository.Object, _mapperMock.Object);
		}

		[Fact]
		public async Task Handler_ShouldReturn_ProperlySortedPagedListOfAlgortihmBasicResponseDto()
		{
			List<AlgorithmTask> resultsFromRepo = new()
			{
				new(Guid.NewGuid() , "" , "" , "" , AdvanceNumber.Create(5)!) ,
				new(Guid.NewGuid() , "" , "" , "" , AdvanceNumber.Create(7)!) ,
			};

			List<AlgorithmTaskBasicResponseDto> mappedResults = new()
			{
				new() {TaskTitle = "Title1"} ,
				new(){TaskTitle = "Title2"}
			};

			_algorithmTaskRepository.Setup(x => x.Query())
				.Returns(resultsFromRepo.BuildMock());

			_mapperMock.Setup(x => x.Map<PagedList<AlgorithmTaskBasicResponseDto>>(It.IsAny<PagedList<AlgorithmTask>>()))
				.Returns(new PagedList<AlgorithmTaskBasicResponseDto>(mappedResults, 1, 1, 1));

			var result = await _handler.Handle(new GetAlgorithmsQuery(new ResourceParamethers()), default);

			result
				.Should()
				.NotBeNull();

			result
				.Should()
				.BeOfType<PagedList<AlgorithmTaskBasicResponseDto>>();

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
