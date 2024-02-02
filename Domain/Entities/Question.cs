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
        public ProperAnswerNumber ProperAnswerNumber { get; private set; } = null!;
        public List<Answer> Answers { get; set; } = [];

        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; } = null!;
        public Guid QuizId { get; set; }

        public Question
            (Guid id, string content, ProperAnswerNumber properAnswerNumber)
            : base(id)
        {
            Content = content;
            ProperAnswerNumber = properAnswerNumber;
        }
    }
}
