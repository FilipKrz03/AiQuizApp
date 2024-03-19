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
	public sealed class Answer : Entity
	{
		public string Content { get; private set; } = string.Empty;
		public AnswerLetter AnswerLetter { get; private set; } 

		[ForeignKey("QuestionId")]
		public Question Question { get; set; } = null!;
		public Guid QuestionId { get; set; }

		public Answer(Guid id, string content , AnswerLetter answerLetter)
			: base(id)
		{
			Content = content;
			AnswerLetter = answerLetter;
		}
	}
}
