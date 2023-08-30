using Blog.Application.Exceptions;
using Blog.Application.UseCases.Commands.Users;
using Blog.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Users
{
    public class EFDeleteUserCommand : EFUseCase, IDeleteUserCommand
    {
        public EFDeleteUserCommand(BlogContext context) : base(context)
        {
        }

        public int Id => 24;

        public string Name => "Delete user";

        public string Description => "Deleting user using Entity Framework.";

        public void Execute(int request)
        {
            // Prvo pronađite korisnika koji će biti obrisan.
            var userToDelete = Context.Users.Find(request);

            if (userToDelete == null)
            {
                throw new EntityNotFoundException("User", request);
            }

          
            var userUseCases = Context.UserUseCases.Where(x => x.UserId == request);
            Context.UserUseCases.RemoveRange(userUseCases);

            var likes = Context.Likes.Where(x => x.UserId == request);
            var likeIds = likes.Select(x => x.Id).ToList();

            var commentsLikes = Context.CommentsLikes.Where(x => likeIds.Contains(x.LikeId));
            Context.CommentsLikes.RemoveRange(commentsLikes);
            Context.SaveChanges();
            var blogPostsLikes = Context.BlogPostsLikes.Where(x => likeIds.Contains(x.LikeId));
            Context.BlogPostsLikes.RemoveRange(blogPostsLikes);
            Context.SaveChanges();
            var comments = Context.Comments.Where(x => x.UserId == request);
            Context.Comments.RemoveRange(comments);

            var blogPosts = Context.BlogPosts.Where(x => x.UserId == request);
            Context.BlogPosts.RemoveRange(blogPosts);

            Context.Likes.RemoveRange(likes);

            Context.Users.Remove(userToDelete);

            
            Context.SaveChanges();
            
        }
    }
}
