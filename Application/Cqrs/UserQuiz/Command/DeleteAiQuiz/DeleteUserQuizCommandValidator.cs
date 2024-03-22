using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Command.DeleteAiQuiz
{
	public class DeleteUserQuizCommandValidator : AbstractValidator<DeleteUserQuizCommand>
	{
		public DeleteUserQuizCommandValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();
		}
	}
}
