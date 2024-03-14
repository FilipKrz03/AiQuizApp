using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class AlgorithmTask : Entity
	{
		public string TaskTitle { get; set; }
		public string TaskContent { get; set; }
		public AdvanceNumber AdvanceNumber { get; set; }
		public List<AlgorithmAnswer> Answers { get; set; } = [];

		public AlgorithmTask(Guid id, string taskTitle, string taskContent, AdvanceNumber advanceNumber)
			: base(id)
		{
			TaskTitle = taskTitle;
			TaskContent = taskContent;
			AdvanceNumber = advanceNumber;
		}
	}
}
