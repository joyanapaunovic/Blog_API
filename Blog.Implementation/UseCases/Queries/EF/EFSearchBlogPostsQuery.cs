using Blog.Application.DTO;
using Blog.Application.DTO.Searches;
using Blog.Application.Exceptions;
using Blog.Application.UseCases.DTO;
using Blog.Application.UseCases.Queries;
using Blog.DataAccess;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Queries.EF
{
    public class EFSearchBlogPostsQuery : EFUseCase, IGetBlogPostsQuery
    {
        
        public EFSearchBlogPostsQuery(BlogContext context) : base(context)
        {

        }

        public int Id => 4;

        public string Name => "Searching blog posts (themes)";

        public string Description => "Searching blog posts with Entity Framework.";

        public PagedResponse<BlogPostDTO> Execute(PageSearch request)
        {
            var query = Context.BlogPosts.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.BlogPostTitle.Contains(request.Keyword));
            }

            query = query.Include(x => x.Categories).ThenInclude(x => x.Category)
                         .Include(x => x.Tags).ThenInclude(x => x.Tag)
                         .Include(x => x.Files).ThenInclude(x => x.File)
                         .Include(x => x.Comments)
                         .ThenInclude(x => x.Likes)
                         .ThenInclude(x => x.Like);

            // po strani default
            if (!request.PerPage.HasValue || request.PerPage < 3)
            {
                request.PerPage = 10;
            }
            
            if (!request.CurrentPage.HasValue || request.CurrentPage < 3)
            {
                request.CurrentPage = 1;
            }

            var toSkip = (request.CurrentPage.Value - 1) * request.PerPage.Value;

            var pagedResponseBlogPost = new PagedResponse<BlogPostDTO>
            {
                TotalCount = query.Count(),
                Items = query.Skip(toSkip).Take(request.PerPage.Value)
                .Select(x => new BlogPostDTO
                {
                    BlogPostTitle = x.BlogPostTitle,
                    Id = x.Id,
                    BlogPostContent = x.BlogPostContent,
                    UserId = x.UserId,
                    Categories = x.Categories.Select(x => new CategoryDTO
                    {
                        Id = x.Category.Id,
                        Title = x.Category.Title,
                        BlogPostsIdsRelatedWithThisCategory = x.Category.BlogPosts.Select(y => y.BlogPostId).ToList()
                    }).ToList(),
                    Tags = x.Tags.Select(x => new TagDTO
                    {
                        Id = x.Tag.Id,
                        TagName = x.Tag.TagName,
                        RelatedBlogPostsIds = x.Tag.BlogPosts.Select(y => y.BlogPostId).ToList()
                    }).ToList(),
                    Files = x.Files.Select(x => new FileDTO
                    {
                        Id = x.File.Id,
                        Path = x.File.Path
                    }).ToList(),
                    Comments = x.Comments.Select(x => new CommentDTO
                    {
                        CommentContent = x.CommentContent,
                        BlogPostId = x.BlogPostId,
                        ParentCommentId = x.ParentCommentId,
                        // ako komentar ima roditeljski id prikazujemo ga
                        // ako nema onda bi trebalo da je null i null je ako je odredjeni komentar main prvi roditelj od koga krece grananje (?)
                        //ParentComment = x.ParentCommentId.HasValue ? new CommentDTO
                        //{
                        //    CommentContent = x.ParentComment.CommentContent,
                        //    BlogPostId = x.ParentComment.BlogPostId,
                        //    CreatedAt = x.ParentComment.CreatedAt,
                        //    UserId = x.ParentComment.UserId,
                        //    Id = x.ParentComment.Id,
                        //    ParentCommentId = x.ParentComment.ParentCommentId,
                        //    LikeIdsOnThisComment = x.ParentComment.Likes.Select(like => like.LikeId).ToList()
                        //} : null,
                        CreatedAt = x.CreatedAt,
                        UserId = x.UserId,
                        Id = x.Id,
                        LikeIdsOnThisComment = x.Likes.Select(y => y.LikeId).ToList()
                    }).ToList(),
                    IdLikesOnThisBlogPost = x.Likes.Select(x => x.LikeId).ToList()
                })
                .ToList(),
                CurrentPage = request.CurrentPage.Value,
                ItemsPerPage = request.PerPage.Value
            };
            return pagedResponseBlogPost;


            //return query.Select(x => new BlogPostDTO
            //                        {
            //                            BlogPostTitle = x.BlogPostTitle,
            //                            Id = x.Id,
            //                            BlogPostContent = x.BlogPostContent,
            //                            UserId = x.UserId,
            //                            Categories = x.Categories.Select(x => new CategoryDTO 
            //                            { 
            //                                Id = x.Category.Id, 
            //                                Title = x.Category.Title 
            //                            }).ToList(),
            //                            Tags = x.Tags.Select(x => new TagDTO 
            //                            { 
            //                                Id = x.Tag.Id, 
            //                                TagName = x.Tag.TagName 
            //                            }).ToList(),
            //                            Files = x.Files.Select(x => new FileDTO 
            //                            { 
            //                                Id = x.File.Id,
            //                                Path = x.File.Path 
            //                            }).ToList()
            //                        })
            //                        .ToList();
        }
    }
}
