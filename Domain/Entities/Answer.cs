using Domain.Common;
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

        [ForeignKey("QuestionId")]
        public Question Question { get; set; } = null!;
        public Guid QuestionId { get; set; }

        public Answer(Guid id, string content)
            : base(id)
        {
            Content = content;
        }
    }
}
