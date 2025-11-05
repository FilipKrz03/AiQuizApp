using Domain.Common;
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
	public class AlgorithmTask : Entity
	{
		public string TaskTitle { get; set; }
		public string TaskMainTopics {  get; set; }	
		public string TaskContent { get; set; }
		public AdvanceNumber AdvanceNumber { get; set; }
		public List<AlgorithmAnswer> Answers { get; set; } = [];
		public CreationStatus CreationStatus { get; set; }


        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        public User? User { get; set; }


        public AlgorithmTask(Guid id, string taskTitle, string taskMainTopics, string taskContent, AdvanceNumber advanceNumber, string? userId = null )
			: base(id)
		{
			TaskTitle = taskTitle;
			TaskMainTopics = taskMainTopics;
			TaskContent = taskContent;
			AdvanceNumber = advanceNumber;
			UserId = userId;
		}
	}
}
