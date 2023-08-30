using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Exceptions
{
    public class InformationExceptionForUser : Exception
    {
        public InformationExceptionForUser(string information) : base(information) 
        {

        }
    }
}
