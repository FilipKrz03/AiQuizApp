using Domain.Enum;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Query.GetAlgorithms
{
	public class GetUserAlgorithmQueryValidator : CreationStatusFilterCorrectnessValidator<GetUserAlgorithmsQuery>
	{
		// All work is done by CreationStatusFilterCorrectnesValidator generic class
	}
}
