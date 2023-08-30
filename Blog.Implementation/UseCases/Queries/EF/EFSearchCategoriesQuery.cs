using Blog.Application.DTO.Searches;
using Blog.Application.UseCases;
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
    public class EFSearchCategoriesQuery : EFUseCase, IGetCategoriesQuery
    {
        public EFSearchCategoriesQuery(BlogContext context) : base(context)
        {
        }

        public int Id => 1;

        public string Name => "Searching categories using Entity Framework.";

        public string Description => "";

        public PagedResponse<CategoryDTO> Execute(PageSearch request)
        {
            var query = Context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Title.Contains(request.Keyword));
            }

            // po strani
            if(!request.PerPage.HasValue || request.PerPage < 3)
            {
                // setujemo koliko ce biti po strani ako korisnik ne prosledi
                request.PerPage = 10;
            }
            // trenutna stranica
            if (!request.CurrentPage.HasValue || request.CurrentPage < 3)
            {
                request.CurrentPage = 1;
            }

            var toSkip = (request.CurrentPage.Value - 1) * request.PerPage.Value;

            query = query.Include(x => x.BlogPosts)
                         .ThenInclude(x => x.BlogPost);

            var pagedResponse = new PagedResponse<CategoryDTO>
            {
                TotalCount = query.Count(),
                Items = query.Skip(toSkip).Take(request.PerPage.Value)
                .Select(x => new CategoryDTO 
                { 
                    Id = x.Id, 
                    Title = x.Title,
                    BlogPostsIdsRelatedWithThisCategory = x.BlogPosts.Select(x => x.BlogPost.Id).ToList()
                }).ToList(),
                CurrentPage = request.CurrentPage.Value,
                ItemsPerPage = request.PerPage.Value
            };
            return pagedResponse;
        }

      
    }
}
