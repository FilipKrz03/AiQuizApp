using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Quiz : Entity
	{
		public string Title { get; private set; } = string.Empty;
		public string TechnologyName { get; private set; } = string.Empty;
		public AdvanceNumber AdvanceNumber { get; private set; }
		public List<Question> Questions { get; set; } = [];

		public Quiz(Guid id, string? title, string technologyName, AdvanceNumber advanceNumber)
		: base(id)
		{
			TechnologyName = technologyName;
			AdvanceNumber = advanceNumber;
			Title = title ?? technologyName + " - " + "Level: " + advanceNumber.Number + "/10";
		}
	}
}
