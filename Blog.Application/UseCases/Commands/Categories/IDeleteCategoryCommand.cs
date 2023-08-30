using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Commands.Categories
{
    public interface IDeleteCategoryCommand : IUseCase, ICommand<int>
    {
    }
}
