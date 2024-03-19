using Application.Common;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Query.GetAlgorithms
{
	public sealed class GetUserAlgorithmsQueryHandler(
		IRepository<UserOwnAlgorithmTask> userOwnAlgorithmTaskRepository,
		IUserRepository userRepository,
		IMapper mapper
		) : IRequestHandler<GetUserAlgorithmsQuery, PagedList<AlgorithmTaskBasicResponseDto>>
	{
		private readonly IRepository<UserOwnAlgorithmTask> _userOwnAlgorithmTaskRepository = userOwnAlgorithmTaskRepository;
		private readonly IUserRepository _userRepository = userRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<PagedList<AlgorithmTaskBasicResponseDto>> Handle(GetUserAlgorithmsQuery request, CancellationToken cancellationToken)
		{
			if (!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			var query = _userOwnAlgorithmTaskRepository
				.Query()
				.Where(x => x.UserId == request.UserId);

			if (!string.IsNullOrWhiteSpace(request.ResourceParamethers.CreationStatus))
			{
				var splittedValues = request.ResourceParamethers.CreationStatus.Split(',');

				List<CreationStatus> creationStatuses = [];

				foreach (var value in splittedValues)
				{
					if (Enum.TryParse(value.Trim(), ignoreCase: true, out CreationStatus statusEnum))
					{
						creationStatuses.Add(statusEnum);
					}
				}

				if (creationStatuses.Count > 0) query = query.Where(x => creationStatuses.Contains(x.CreationStatus));
			}

			if (!string.IsNullOrWhiteSpace(request.ResourceParamethers.SearchQuery))
			{
				query = query.Where(
					x => x.TaskMainTopics.Contains(request.ResourceParamethers.SearchQuery) ||
					x.TaskContent.Contains(request.ResourceParamethers.SearchQuery) ||
					x.TaskTitle.Contains(request.ResourceParamethers.SearchQuery) ||
					x.AdvanceNumber.ToString().Contains(request.ResourceParamethers.SearchQuery)
				);
			}

			Expression<Func<UserOwnAlgorithmTask, object>> keySelector =
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

			var result = await PagedList<UserOwnAlgorithmTask>.CreateAsync(
				query,
				request.ResourceParamethers.PageSize,
				request.ResourceParamethers.PageNumber
				);

			return _mapper.Map<PagedList<AlgorithmTaskBasicResponseDto>>(result)!;
		}
	}
}
