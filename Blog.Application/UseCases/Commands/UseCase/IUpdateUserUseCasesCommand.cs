using Blog.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Commands.UseCase
{
    public interface IUpdateUserUseCasesCommand : IUseCase, ICommand<UpdateUserUseCasesDTO>
    {
    }
}
