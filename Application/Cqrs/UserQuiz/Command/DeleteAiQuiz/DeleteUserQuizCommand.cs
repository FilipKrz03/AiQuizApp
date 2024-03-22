using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Command.DeleteAiQuiz
{
	public sealed record DeleteUserQuizCommand(Guid QuizId , string UserId) : IRequest
	{
	}
}
