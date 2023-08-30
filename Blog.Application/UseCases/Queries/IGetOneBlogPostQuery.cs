using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Application.UseCases.DTO;

namespace Blog.Application.UseCases.Queries
{
    public interface IGetOneBlogPostQuery : IUseCase, IQuery<int, BlogPostDTO>
    {
    }
}
