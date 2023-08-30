using Blog.Application.DTO;
using Blog.Application.DTO.Searches;
using Blog.Application.UseCases.DTO;
using Blog.Application.UseCases.Queries;
using Blog.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Queries.EF
{
    public class EFSearchTagsQuery : EFUseCase, IGetTagsQuery
    {
        public EFSearchTagsQuery(BlogContext context) : base(context)
        {
        }

        public int Id => 8;

        public string Name => "Searching tags";

        public string Description => "Searching tags using EF.";

        public PagedResponse<TagDTO> Execute(PageSearch request)
        {
            var query = Context.Tags.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.TagName.Contains(request.Keyword));
            }

            
            if (!request.PerPage.HasValue || request.PerPage < 3)
            {
                
                request.PerPage = 10;
            }
            
            if (!request.CurrentPage.HasValue || request.CurrentPage < 3)
            {
                request.CurrentPage = 1;
            }

            var toSkip = (request.CurrentPage.Value - 1) * request.PerPage.Value;
            query = query.Include(x => x.BlogPosts)
                         .ThenInclude(x => x.BlogPost);
            var pagedResponseTags = new PagedResponse<TagDTO>
            {
                TotalCount = query.Count(),
                Items = query.Skip(toSkip).Take(request.PerPage.Value)
                .Select(x => new TagDTO
                {
                    Id = x.Id,
                    TagName = x.TagName,
                    RelatedBlogPostsIds = x.BlogPosts.Select(x => x.BlogPost.Id).ToList()
                }).ToList(),
                CurrentPage = request.CurrentPage.Value,
                ItemsPerPage = request.PerPage.Value
            };
            return pagedResponseTags;
        }
    }
}
