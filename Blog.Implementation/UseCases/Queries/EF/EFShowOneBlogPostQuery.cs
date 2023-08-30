using Blog.Application.UseCases.DTO;
using Blog.Application.UseCases.Queries;
using Blog.DataAccess;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Blog.Application.DTO;

namespace Blog.Implementation.UseCases.Queries.EF
{
    public class EFShowOneBlogPostQuery : EFUseCase, IGetOneBlogPostQuery
    {
        public EFShowOneBlogPostQuery(BlogContext context) : base(context)
        {
        }

        public int Id => 16;

        public string Name => "Show details about one blog post";

        public string Description => "Showing one blog post by specific id using Entity Framework.";

        public BlogPostDTO Execute(int request)
        {
            var blogPost = Context.BlogPosts.Include(x => x.Categories).ThenInclude(x => x.Category)
                               .Include(x => x.Tags).ThenInclude(x => x.Tag)
                               .Include(x => x.Files).ThenInclude(x => x.File)
                               .Include(x => x.Comments)
                               .ThenInclude(x => x.Likes)
                               .ThenInclude(x => x.Like)
                               .Where(x => x.Id == request)
                               .FirstOrDefault();
            var showBlogPost = new BlogPostDTO
            {
                Id = blogPost.Id,
                BlogPostTitle = blogPost.BlogPostTitle,
                BlogPostContent = blogPost.BlogPostContent,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Category.Id,
                    Title = x.Category.Title
                }).ToList(),
                Tags = blogPost.Tags.Select(x =>  new TagDTO
                {
                    Id = x.Tag.Id,
                    TagName = x.Tag.TagName
                }).ToList(),
                Files = blogPost.Files.Select(x => new FileDTO {
                    Id = x.File.Id,
                    Path = x.File.Path
                }).ToList(),
                IdLikesOnThisBlogPost = blogPost.Likes.Select(x => x.LikeId).ToList(),
                Comments = blogPost.Comments.Select(x => new CommentDTO
                {
                    Id = x.Id,
                    CommentContent = x.CommentContent,
                    UserId = x.UserId,
                    BlogPostId = x.BlogPostId,
                    ParentCommentId = x.ParentCommentId,
                    CreatedAt = x.CreatedAt,
                    LikeIdsOnThisComment = x.Likes.Select(x => x.LikeId).ToList()
                }).ToList()
            };

            return showBlogPost;

        }
    }
}
