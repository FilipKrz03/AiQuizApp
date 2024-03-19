using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
	public record AnswerLetter
	{
		private AnswerLetter(char letter) => Letter = letter;

		public char Letter { get; init; }

		public static AnswerLetter? Create(char letter)
		{
			if(letter != 'a' && letter != 'b' && letter != 'c' && letter != 'd')
			{
				return null;
			}

			return new AnswerLetter(letter);
		}
	}
}
