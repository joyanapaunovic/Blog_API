using Blog.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Commands.Blog_posts
{
    public interface ICreateBlogPostCommand : IUseCase, ICommand<CreateBlogPostDTO>
    {
    }
}
