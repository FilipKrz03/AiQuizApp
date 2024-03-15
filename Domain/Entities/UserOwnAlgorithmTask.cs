using Domain.Enum;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class UserOwnAlgorithmTask : AlgorithmTask
	{
		public CreationStatus CreationStatus { get; set; }

		[ForeignKey("UserId")]
		public string UserId { get; set; } = null!;
		public User User { get; set; } = null!;

		public UserOwnAlgorithmTask(Guid id, string taskTitle, string taskMainTopics, string taskContent, AdvanceNumber advanceNumber)
			: base(id, taskTitle, taskMainTopics, taskContent, advanceNumber)
		{
		}
	}
}
