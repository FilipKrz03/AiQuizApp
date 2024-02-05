using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(Guid id, string objectName = "Object")
        : base($"{objectName} with id {id} not found") { }
    }
}
