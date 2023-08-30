using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Application.DTO;
using Blog.Application.UseCases.Commands.Comments;
using Blog.DataAccess;
using Blog.Implementation.Validators;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Blog.Application.Exceptions;
using System.Net;
using Microsoft.EntityFrameworkCore;
namespace Blog.Implementation.UseCases.Commands.EF.Comments
{
    public class EFCreateCommentCommand : EFUseCase, ICreateCommentCommand
    {
        private readonly CreateCommentValidator _commentValidator;
        public EFCreateCommentCommand(BlogContext context, 
                                      CreateCommentValidator commentValidator) : base(context)
        {
            _commentValidator = commentValidator;
        }

        public int Id => 17;

        public string Name => "Create comment(s)";

        public string Description => "Creating comment(s) using Entity Framework.";

        public void Execute(CommentDTO request)
        {
            // VALIDACIJA
            _commentValidator.ValidateAndThrow(request);
         
            
                var blogPostId = request.BlogPostId.GetValueOrDefault();
                // parent comment id - da li postoji
                if (request.ParentCommentId.HasValue)
                {
                    var parentComment = Context.Comments.Find(request.ParentCommentId.Value);
                    blogPostId = parentComment.BlogPostId;
                }

                Context.Comments.Add(new Domain.Comment
                {
                    CommentContent = request.CommentContent,
                    UserId = request.UserId,
                    BlogPostId = blogPostId,
                    ParentCommentId = request.ParentCommentId
                });
                request.CreatedAt = DateTime.UtcNow;

                Context.SaveChanges();

            




        }
    }
}
