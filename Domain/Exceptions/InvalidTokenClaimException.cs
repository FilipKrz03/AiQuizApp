using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
	public class InvalidTokenClaimException : Exception
	{
		public InvalidTokenClaimException()
		   : base("User with id from your token do not exist") { }
	}
}
