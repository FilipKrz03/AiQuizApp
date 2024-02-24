using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Command.CreateQuizCommand
{
	public sealed record CreateQuizCommand(string TechnologyName , int AdvanceNumber) : IRequest<string>
	{
	}
}
