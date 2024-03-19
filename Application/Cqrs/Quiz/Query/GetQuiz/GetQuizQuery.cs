using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.Quiz.Query.GetQuiz
{
	public sealed record GetQuizQuery(Guid Id) : IRequest<QuizDetailResponseDto>
	{
	}
}
