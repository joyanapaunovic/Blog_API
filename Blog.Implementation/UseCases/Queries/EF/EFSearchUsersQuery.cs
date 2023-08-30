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
    public class EFSearchUsersQuery : EFUseCase, IGetUsersQuery
    {
        public EFSearchUsersQuery(BlogContext context) : base(context)
        {
        }

        public int Id => 10;

        public string Name => "Search users";

        public string Description => "Searching users using Entity Framework.";

        public PagedResponse<CommonUserDTO> Execute(PageSearch request)
        {
           
            var query = Context.Users.AsQueryable();
           
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.FirstName.Contains(request.Keyword)
                                      || x.LastName.Contains(request.Keyword));
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
            
            // include
            query = query.Include(x => x.UseCases)
                         .Include(x => x.BlogPosts);

                var users = new PagedResponse<CommonUserDTO>();
                // ako admin pretrazuje korisnike, prikazujemo mu
                // korisnike i njegove id-eve dozvoljenih slucajeva koriscenja
                
                var currentUser = query.Where(x => x.Id != 3).Any(x => x.Id == request.CurrentUser);
            
                if (!currentUser)
                {
                users = new PagedResponse<CommonUserDTO>
                {
                    TotalCount = query.Count(),
                    Items = query.Skip(toSkip).Take(request.PerPage.Value)
                   .Select(x => new CommonUserDTO
                   {
                       Id = x.Id,
                       FirstName = x.FirstName,
                       LastName = x.LastName,
                       Email = x.Email,
                       Password = x.Password,
                       BlogPostsIdsRelatedToThisUser = x.BlogPosts
                          .Where(y => y.UserId == x.Id)
                          .Select(y => y.Id).ToList(),
                       UseCasesIdsAllowedForThisUser = x.UseCases.Select(x => x.UseCaseId).ToList()
                    }).ToList(),
                    CurrentPage = request.CurrentPage.Value,
                    ItemsPerPage = request.PerPage.Value
                };
            }
            // ukoliko korisnici pretrazuju korisnike, prikazujemo podatke o korisniku i njegove blogove
            else
            {
                users = new PagedResponse<CommonUserDTO>
                {
                   TotalCount = query.Count(),
                   Items = query.Skip(toSkip).Take(request.PerPage.Value)
                  .Where(x => x.Id != 3)
                  .Select(x => new CommonUserDTO
                  {
                      Id = x.Id,
                      FirstName = x.FirstName,
                      LastName = x.LastName,
                      Email = x.Email,
                      BlogPostsIdsRelatedToThisUser = x.BlogPosts
                      .Where(y => y.UserId == x.Id)
                      .Select(y => y.Id).ToList()
                      }).ToList(),
                        CurrentPage = request.CurrentPage.Value,
                        ItemsPerPage = request.PerPage.Value
                };
            }
            return users;
        }
    }
}
