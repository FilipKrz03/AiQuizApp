using Application.Interfaces;
using Domain.Enum;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Query.GetAlgorithms
{
	public class CreationStatusFilterCorrectnessValidator<T> : AbstractValidator<T>
		where T : IResourceParamethersWithCreationStatus
	{
		public CreationStatusFilterCorrectnessValidator()
		{
			RuleFor(x => CheckCreationStatusStringCorrectness(x.ResourceParamethers.CreationStatus)).Equal(true)
				.WithMessage(
				"If you want to include filtering by creation status you should provide at least one proper value of " +
				": Pending , Succes , Failed " +
				"eg . CreationStatus=Succes or CreationStatus=Pending,Failed (if multiple values they should be separet by comma"
				);
		}

		private bool CheckCreationStatusStringCorrectness(string? creationStatusString)
		{
			if (creationStatusString == null) return true;

			List<string> craetionStatusesAsStrings = new()
			{
				CreationStatus.Succes.ToString() ,
				CreationStatus.Pending.ToString() ,
				CreationStatus.Failed.ToString() ,
			};

			var creationStatusesProvidedByUser = creationStatusString.Split(',');

			foreach (var creationStatus in creationStatusesProvidedByUser)
			{
				if (craetionStatusesAsStrings.Contains(creationStatus.Trim(), StringComparer.OrdinalIgnoreCase)) return true;
			}

			return false;
		}
	}
}
