using Application.Common;
using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Query.GetUserAiQuizesQuery
{
	public sealed record GetUserAiQuizesQuery(string UserId , ResourceParamethers ResourceParamethers)
		: IRequest<PagedList<QuizBasicResponseDto>>
	{
	}
}
