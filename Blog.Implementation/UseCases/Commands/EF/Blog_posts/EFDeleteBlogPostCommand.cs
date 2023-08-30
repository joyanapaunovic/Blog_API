using Blog.Application.DTO;
using Blog.Application.Exceptions;
using Blog.Application.UseCases.Commands.Blog_posts;
using Blog.Application.UseCases.DTO;
using Blog.Application.UseCases.Queries;
using Blog.DataAccess;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Blog_posts
{
    public class EFDeleteBlogPostCommand : EFUseCase, IDeleteBlogPostCommand
    {
        public EFDeleteBlogPostCommand(BlogContext context) : base(context)
        {
        }

        public int Id => 6;

        public string Name => "Delete a blog post";

        public string Description => "Deleting a blog post using Entity Framework.";



        public void Execute(int request, UserDTO user)
        {
            //Console.WriteLine("Id blog post-a" + request);
            //Console.WriteLine("Id ulogovanog korisnika" + user.Id);
            // BLOG POSTS CATEGORIES
            var categories = Context.BlogPostsCategories.Where(x => x.BlogPostId == request);
            Context.BlogPostsCategories.RemoveRange(categories);
            // BLOG POSTS TAGS
            var tags = Context.BlogPostsTags.Where(x => x.BlogPostId == request);
            Context.BlogPostsTags.RemoveRange(tags);
            // BLOG POSTS FILES
            var files = Context.BlogPostsFiles.Where(x => x.BlogPostId == request);
            Context.BlogPostsFiles.RemoveRange(files);

            // DELETE THE FILES
            var fileIds = files.Select(x => x.FileId).ToList();
            var filesToDelete = Context.Files.Where(f => fileIds.Contains(f.Id));
            Context.Files.RemoveRange(filesToDelete);

            // LIKES ON COMMENTS
            var comments = Context.Comments.Where(x => x.BlogPostId == request).ToList();
            var commentIds = comments.Select(x => x.Id).ToList();
            var parentCommentIds = comments.Select(x => x.ParentCommentId).ToList();

            var parentCommentsLikes = Context.CommentsLikes
                                            .Where(x => parentCommentIds.Contains(x.CommentId));
            var childCommentsLikes = Context.CommentsLikes
                                            .Where(x => commentIds.Contains(x.CommentId));

            if (parentCommentsLikes != null)
            {
                Context.CommentsLikes.RemoveRange(parentCommentsLikes);
            }
            if (childCommentsLikes != null)
            {
                Context.CommentsLikes.RemoveRange(childCommentsLikes);
            }
            // COMMENTS
            if (comments.Any())
            {
                Context.Comments.RemoveRange(comments);
            }

            // BLOG POST


            var blogPost = Context.BlogPosts.Where(x => x.Id == request && x.UserId == user.Id).FirstOrDefault();

            if (blogPost != null)
            {
                Context.BlogPosts.Remove(blogPost);
            }
            else if (blogPost == null)
            {
                throw new InformationExceptionForUser("Dear user, you don't have a blog post with that id, so we didn't complete the deletion. " +
                "Check again your blog posts and try again.");
            }
            else
            {
                throw new EntityNotFoundException(nameof(BlogPost), request);
            }

            Context.SaveChanges();

        }


    }
}
