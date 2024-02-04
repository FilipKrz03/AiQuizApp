using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record AdvanceNumber
    {
        private AdvanceNumber(int number) => Number = number;

        public int Number { get; init; }

        public static AdvanceNumber? Create(int number)
        {
            if (number < 0 || number > 10)
            {
                return null;
            }

            return new AdvanceNumber(number);
        }
    }
}
