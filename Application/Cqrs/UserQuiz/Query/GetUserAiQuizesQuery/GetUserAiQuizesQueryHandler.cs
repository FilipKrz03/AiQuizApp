using Application.Common;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Query.GetUserAiQuizesQuery
{
	public sealed class GetUserAiQuizesQueryHandler(
		IUserRepository userRepository,
		IRepository<UserOwnQuiz> userOwnQuizRepository,
		IMapper mapper
		) : IRequestHandler<GetUserAiQuizesQuery, PagedList<QuizBasicResponseDto>>
	{
		private readonly IUserRepository _userRepository = userRepository;
		private readonly IRepository<UserOwnQuiz> _userOwnQuizRepository = userOwnQuizRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<PagedList<QuizBasicResponseDto>> Handle(GetUserAiQuizesQuery request, CancellationToken cancellationToken)
		{
			if (!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			var userOwnQuizesQuery = _userOwnQuizRepository
				.Query()
				.Where(x => x.UserId == request.UserId);

			if (!request.ResourceParamethers.SearchQuery.IsNullOrEmpty())
			{
				userOwnQuizesQuery = userOwnQuizesQuery
					.Where(
						x => x.Title.Contains(request.ResourceParamethers.SearchQuery!) ||
						x.AdvanceNumber.Number.ToString().Contains(request.ResourceParamethers.SearchQuery!) ||
						x.TechnologyName.Contains(request.ResourceParamethers.SearchQuery!)
						);
			}

			Expression<Func<UserOwnQuiz, object>> keySelector =
				request.ResourceParamethers.SortColumn switch
				{
					"title" => quiz => quiz.Title,
					"technologyName" => quiz => quiz.TechnologyName,
					"advanceNumber" => quiz => quiz.AdvanceNumber.Number,
					_ => quiz => quiz.Id
				};

			if (request.ResourceParamethers.SortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
			{
				userOwnQuizesQuery = userOwnQuizesQuery.OrderByDescending(keySelector);
			}
			else
			{
				userOwnQuizesQuery = userOwnQuizesQuery.OrderBy(keySelector);
			}
		
			var quizesFromDb = await PagedList<UserOwnQuiz>.CreateAsync(
				userOwnQuizesQuery,
				request.ResourceParamethers.PageSize,
				request.ResourceParamethers.PageNumber
				);

			return _mapper.Map<PagedList<QuizBasicResponseDto>>(quizesFromDb)!;
		}
	}
}
