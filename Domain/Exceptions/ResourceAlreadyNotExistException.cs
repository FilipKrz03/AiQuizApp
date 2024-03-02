using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
	public class ResourceAlreadyNotExistException : Exception
	{
		public ResourceAlreadyNotExistException(Guid id, string objectName = "Object")
			: base($"{objectName} with id {id} already do not exist") { }
	}
}
