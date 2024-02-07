﻿using Application.Common;
using Application.Dto;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.Quiz.Query.GetQuizes
{
    public sealed class GetQuizesQueryHandler : IRequestHandler<GetQuizesQuery, PagedList<QuizBasicResponseDto>>
    {
        private readonly IRepository<Domain.Entities.Quiz> _quizRepository;
        private readonly IMapper _mapper;

        public GetQuizesQueryHandler(
            IRepository<Domain.Entities.Quiz> quizRepository,
            IMapper mapper
            )
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<QuizBasicResponseDto>> Handle(GetQuizesQuery request, CancellationToken cancellationToken)
        {
            var query = _quizRepository.Query();

            if (!string.IsNullOrWhiteSpace(request.ResourceParamethers.SearchQuery))
            {
                query = query.Where(x =>
                x.Title.Contains(request.ResourceParamethers.SearchQuery) ||
                x.TechnologyName.Contains(request.ResourceParamethers.SearchQuery) || 
                x.Questions.Any(q => q.Content.Contains(request.ResourceParamethers.SearchQuery))
                );
            }

            Expression<Func<Domain.Entities.Quiz, object>> keySelector
                = request.ResourceParamethers.SortColumn switch
                {
                    "title" => quiz => quiz.Title,
                    "technologyName" => quiz => quiz.TechnologyName,
                    _ => quiz => quiz.Id
                };

            if (request.ResourceParamethers.SortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
            {
                query = query.OrderByDescending(keySelector);
            }
            else
            {
                query = query.OrderBy(keySelector);
            }

            var pagedList = await PagedList<Domain.Entities.Quiz>
                .CreateAsync(query,
                request.ResourceParamethers.PageSize,
                request.ResourceParamethers.PageNumber
                );

            return _mapper.Map<PagedList<QuizBasicResponseDto>>(pagedList)!;
        }
    }
}
