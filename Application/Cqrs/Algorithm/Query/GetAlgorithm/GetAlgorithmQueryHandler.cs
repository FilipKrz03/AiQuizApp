using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.Algorithm.Query.GetAlgorithm
{
	public class GetAlgorithmQueryHandler(
		IRepository<AlgorithmTask> algorithmTaskRepository,
		IMapper mapper
		) : IRequestHandler<GetAlgorithmQuery, AlgorithmTaskDetailResponseDto>
	{
		private readonly IRepository<AlgorithmTask> _algorithmTaskRepository = algorithmTaskRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<AlgorithmTaskDetailResponseDto> Handle(GetAlgorithmQuery request, CancellationToken cancellationToken)
		{
			var algorithm = await _algorithmTaskRepository
				.GetByIdQuery(request.AlgorithmId)
				.Include(x => x.Answers)
				.FirstOrDefaultAsync();

			if(algorithm == null)
			{
				throw new ResourceNotFoundException(request.AlgorithmId, "Algorithm");
			}

			return _mapper.Map<AlgorithmTaskDetailResponseDto>(algorithm)!;
		}
	}
}
