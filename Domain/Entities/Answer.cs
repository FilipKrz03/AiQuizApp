using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Answer : Entity
    {
        public string Content { get; private set; } = string.Empty;

        public Answer(Guid id, string content)
            : base(id)
        {
            Content = content;
        }
    }
}
