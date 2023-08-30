using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Commands.Tags
{
    public interface IDeleteTagCommand : IUseCase, ICommand<int>
    {
    }
}
