using Blog.Application.DTO;
using Blog.Application.UseCases.Commands.Comments;
using Blog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Blog.Implementation.Validators;
using Blog.Application.Exceptions;
using Blog.Domain;

namespace Blog.Implementation.UseCases.Commands.EF.Comments
{
    public class EFUpdateCommentCommand : EFUseCase, IUpdateCommentCommand
    {
        private readonly CreateCommentValidator _validator;
        public EFUpdateCommentCommand(BlogContext context, CreateCommentValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 18;

        public string Name => "Update comment using Entity Framework.";

        public string Description => "Updating comment using Entity Framework.";

        public void Execute(CommentDTO request)
        {
            Console.WriteLine("Comment id" + request.Id);
            var userId = Context.Comments.Where(x => x.Id == request.Id)
                                         .Any(x => x.UserId == request.UserId);
            if(!userId)
            {
                throw new InformationExceptionForUser("Dear user, it seems that the comment " +
                    "you've wanted to update isn't yours. " +
                    "Please check all the required things for your update and try again.");
            }
            
                // validacija
                _validator.Validate(request);
                var commentToUpdate = Context.Comments.Find(request.Id);
                // id, userId
                commentToUpdate.Id = request.Id;
                commentToUpdate.UserId = request.UserId;
                // blog post id, existing (if) parentCommentId
                var blogPostId = request.BlogPostId.GetValueOrDefault();
                commentToUpdate.BlogPostId = blogPostId;
                commentToUpdate.CommentContent = request.CommentContent;
                commentToUpdate.ParentCommentId = request.ParentCommentId;
                var getUserDataFromTheRequestUserId = Context.Users.Find(request.UserId);
                // updated by, updated at
                commentToUpdate.UpdatedBy = getUserDataFromTheRequestUserId.Email;
                commentToUpdate.UpdatedAt = DateTime.UtcNow;

                Context.SaveChanges();
            
        }
    }
}
