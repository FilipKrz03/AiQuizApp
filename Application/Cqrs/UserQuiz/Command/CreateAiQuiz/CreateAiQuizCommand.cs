using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Command.CreateAiQuiz
{
	public sealed record CreateAiQuizCommand(string UserId , string TechnologyName, 
		int AdvanceNumber, string? QuizTitle) : IRequest
	{
	}
}
