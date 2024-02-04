using Domain.Common;
using Domain.ValueObjects;
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
        public string TechnologyName { get; private set; } = string.Empty;
        public AdvanceNumber AdvanceNumber { get; private set; }
        public List<Question> Questions { get; set; } = [];

        public Quiz(Guid id, string title, string technologyName, AdvanceNumber advanceNumber)
        : base(id)
        {
            Title = title;
            TechnologyName = technologyName;
            AdvanceNumber = advanceNumber;
        }
    }
}
