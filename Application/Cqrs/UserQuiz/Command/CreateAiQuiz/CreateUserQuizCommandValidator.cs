using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Command.CreateAiQuiz
{
	public class CreateUserQuizCommandValidator : AbstractValidator<CreateUserQuizCommand>
	{
		public CreateUserQuizCommandValidator()
		{
			RuleFor(x => x.AdvanceNumber).NotEmpty().GreaterThan(0).LessThanOrEqualTo(10);
			RuleFor(x => x.QuizTitle).NotEmpty();
			RuleFor(x => x.TechnologyName).NotEmpty();
			RuleFor(x => x.UserId).NotEmpty();
		}
	}
}
