using Blog.Application.DTO;
using Blog.DataAccess;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.Validators
{
    public class CreateCommentValidator : AbstractValidator<CommentDTO>
    {
        private readonly BlogContext _blogContext;
        public CreateCommentValidator(BlogContext context) 
        {
            _blogContext = context;
            // comment content
            RuleFor(x => x.CommentContent)
                          .Cascade(CascadeMode.Stop)
                          .NotEmpty().WithMessage("Comment field can't be empty.")
                          .MinimumLength(3).WithMessage("Minimum length for the comment field is 3 characters.")
                          .MaximumLength(300).WithMessage("Maximum length for the comment field is 300 characters.");
            // blog post id
            RuleFor(x => x.BlogPostId).Cascade(CascadeMode.Stop)
                                      .NotEmpty()
                                        .WithMessage("Please enter the blog post id you want to comment.")
                                      .Must(x => context.BlogPosts.Any(c => c.Id == x))
                                        .WithMessage("Please enter the existing blog post id. ");
            // user id
            RuleFor(x => x.UserId).Cascade(CascadeMode.Stop).NotEmpty()
                                                            .WithMessage("Please check your data " +
                                                            "and enter your user id. ")
                                                            .Must(x => context.Users.Any(c => c.Id == x))
                                                            .WithMessage("Dear user, the id that you entered doesn't seem to match your user id. Please check your data and try again. "); ;
        }

    }
}
