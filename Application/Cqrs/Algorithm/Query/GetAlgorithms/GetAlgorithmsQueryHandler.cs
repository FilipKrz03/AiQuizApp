using Application.Common;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.Algorithm.Query.GetAlgorithms
{
	public sealed class GetAlgorithmsQueryHandler(
		IRepository<AlgorithmTask> algorithmTaskRepository,
		IMapper mapper
		) : IRequestHandler<GetAlgorithmsQuery, PagedList<AlgorithmTaskBasicResponseDto>>
	{
		private readonly IRepository<AlgorithmTask> _algorithmTaskRepository = algorithmTaskRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<PagedList<AlgorithmTaskBasicResponseDto>> Handle(GetAlgorithmsQuery request, CancellationToken cancellationToken)
		{
			var query = _algorithmTaskRepository
				.Query()
				.Where(x => !(x is UserOwnAlgorithmTask));

			if (!string.IsNullOrWhiteSpace(request.ResourceParamethers.SearchQuery))
			{
				query = query.Where(
					x => x.TaskMainTopics.Contains(request.ResourceParamethers.SearchQuery) ||
					x.TaskContent.Contains(request.ResourceParamethers.SearchQuery) ||
					x.TaskTitle.Contains(request.ResourceParamethers.SearchQuery) 
				);
			}

			Expression<Func<AlgorithmTask, object>> keySelector =
				request.ResourceParamethers.SortColumn switch
				{
					"taskMainTopics" => algorithm => algorithm.TaskMainTopics,
					"taskContent" => algorithm => algorithm.TaskContent,
					"taskTitle" => algorithm => algorithm.TaskTitle,
					"advanceNumber" => algorithm => algorithm.AdvanceNumber,
					_ => algorithm => algorithm.Id
				};

			if (request.ResourceParamethers.SortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
			{
				query = query.OrderByDescending(keySelector);
			}
			else
			{
				query = query.OrderBy(keySelector);
			}

			var result = await PagedList<AlgorithmTask>.CreateAsync(
				query,
				request.ResourceParamethers.PageSize,
				request.ResourceParamethers.PageNumber
				);

			return _mapper.Map<PagedList<AlgorithmTaskBasicResponseDto>>(result)!;
		}
	}
}
