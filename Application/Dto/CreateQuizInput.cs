using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public record class CreateQuizInput(string technologyName, AdvanceNumber advanceNumber, string? quizTitle)
	{
		public override string ToString()
		{
			return $"{technologyName}";
		}
	}
}
