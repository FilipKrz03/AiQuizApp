using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Quiz : Entity
    {
        public string Title { get; private set; } = string.Empty;
        public List<Question> Questions { get;  set; } = [];

        public Quiz(Guid id, string title)
        : base(id)
        {
            Title = title;
        }
    }
}
