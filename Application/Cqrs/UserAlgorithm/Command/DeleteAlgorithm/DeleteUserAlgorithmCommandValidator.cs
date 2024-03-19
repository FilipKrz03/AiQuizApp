using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Command.DeleteAlgorithm
{
	public class DeleteUserAlgorithmCommandValidator : AbstractValidator<DeleteUserAlgorithmCommand>
	{
        public DeleteUserAlgorithmCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
