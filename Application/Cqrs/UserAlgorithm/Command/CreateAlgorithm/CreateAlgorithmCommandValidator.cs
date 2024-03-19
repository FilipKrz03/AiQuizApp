using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Command.CreateAlgorithm
{
	public class CreateAlgorithmCommandValidator : AbstractValidator<CreateAlgorithmCommand>
	{
        public CreateAlgorithmCommandValidator()
        {
            RuleFor(x => x.AdvanceNumber).NotEmpty().GreaterThan(0).LessThanOrEqualTo(10);
            RuleFor(x => x.TaskMainTopics).NotEmpty().MinimumLength(2);
            RuleFor(x => x.TaskTitle).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
