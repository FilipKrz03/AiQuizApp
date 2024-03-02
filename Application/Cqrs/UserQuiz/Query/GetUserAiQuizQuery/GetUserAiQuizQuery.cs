using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Query.GetUserAiQuizQuery
{
	public sealed record GetUserAiQuizQuery(Guid QuizId , string UserId) : IRequest<QuizDetailResponseDto>
	{
	}
}
