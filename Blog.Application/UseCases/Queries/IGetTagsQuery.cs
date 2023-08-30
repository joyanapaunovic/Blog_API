using Blog.Application.DTO;
using Blog.Application.DTO.Searches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Queries
{
    public interface IGetTagsQuery : IUseCase, IQuery<PageSearch, PagedResponse<TagDTO>>
    {
    }
}
