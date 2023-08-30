using Blog.Application.DTO;
using Blog.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Queries
{
    public interface IGetCommentsQuery : IUseCase, IQuery<int, BlogPostWithOnlyIdAndParentCommentWithChildrenDTO>
    {
    }
}
