using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Question : Entity
    {
        public string Content { get; private set; } = string.Empty;
        public List<Answer> Answers { get; private set; } = [];
        public ProperAnswerNumber ProperAnswerNumber { get; private set; } = null!;

        public Question
            (Guid id, string content, List<Answer> answers, ProperAnswerNumber properAnswerNumber)
            : base(id)
        {
            Content = content;
            Answers = answers;
            ProperAnswerNumber = properAnswerNumber;
        }
    }
}
