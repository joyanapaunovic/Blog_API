using Blog.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Commands.Users
{
    public interface IRegisterUserCommand : IUseCase, ICommand<RegisterUserDTO>
    {
    }
}
