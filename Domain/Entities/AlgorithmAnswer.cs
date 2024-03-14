using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class AlgorithmAnswer : Entity
	{
		public string AnswerContent { get; set; }
		public string Language { get; set; }

		[ForeignKey("AlgorithmTaskId")]
		public AlgorithmTask AlgorithmTask { get; set; } = default!;
		public Guid AlgorithmTaskId { get; set; }

		public AlgorithmAnswer(Guid id, string answerContent, string language)
			: base(id)
		{
			Language = language;
			AnswerContent = answerContent;
		}
	}
}
