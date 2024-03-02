using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
	public class InvalidAccesTokenException : Exception
	{
		public InvalidAccesTokenException()
			: base("Provided access token is invalid") { }
	}
}
