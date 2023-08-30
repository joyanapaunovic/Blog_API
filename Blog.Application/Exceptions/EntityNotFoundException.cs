using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, int id)
        : base($"Entity {entityName} with an id {id} was not found.")
        { 
            
        }
    }
}
