using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record ProperAnswerNumber
    {
        private ProperAnswerNumber(int number) => Number = number;

        public int Number { get; init; }

        public static ProperAnswerNumber? Create(int number)
        {
            if (number < 0 || number > 4)
            {
                return null;
            }

            return new ProperAnswerNumber(number);
        }
    }
}
