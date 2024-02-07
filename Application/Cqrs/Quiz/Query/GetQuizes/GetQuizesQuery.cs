using Application.Common;
using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.Quiz.Query.GetQuizes
{
    public sealed record GetQuizesQuery(ResourceParamethers ResourceParamethers) 
        : IRequest<PagedList<QuizBasicResponseDto>>
    { 
    }
}
