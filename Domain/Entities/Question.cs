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
    public sealed class Question : Entity
    {
        public string Content { get; private set; } = string.Empty;
        public AnswerLetter ProperAnswerLetter { get; private set; } = null!;
        public List<Answer> Answers { get; set; } = [];

        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; } = null!;
        public Guid QuizId { get; set; }

        public Question
            (Guid id, string content, AnswerLetter properAnswerLetter)
            : base(id)
        {
            Content = content;
            ProperAnswerLetter = properAnswerLetter;
        }
    }
}
