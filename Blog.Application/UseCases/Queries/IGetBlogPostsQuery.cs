using Blog.Application.DTO.Searches;
using Blog.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Queries
{
    public interface IGetBlogPostsQuery : IUseCase, IQuery<PageSearch, PagedResponse<BlogPostDTO>>
    {
    }
}
