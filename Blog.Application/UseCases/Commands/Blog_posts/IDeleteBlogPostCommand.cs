using Blog.Application.DTO;
using Blog.Application.UseCases.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Commands.Blog_posts
{
    public interface IDeleteBlogPostCommand : IUseCase, ICommandWithSecondParameter<int, UserDTO>
    {
    }
}
