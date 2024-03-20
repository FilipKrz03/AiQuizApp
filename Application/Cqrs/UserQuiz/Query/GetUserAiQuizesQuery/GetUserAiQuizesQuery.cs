using Application.Common;
using Application.Dto;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Query.GetUserAiQuizesQuery
{
	public sealed record GetUserAiQuizesQuery(string UserId , ResourceParamethersWithCreationStatus ResourceParamethers)
		: IRequest<PagedList<UserOwnQuizBasicResponseDto>> , IResourceParamethersWithCreationStatus
	{
	}
}
