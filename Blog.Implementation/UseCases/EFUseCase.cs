using Blog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases
{
    public abstract class EFUseCase
    {
        
        protected EFUseCase(BlogContext context)
        {
            Context = context;
        }

        protected BlogContext Context { get; }
    }
}
