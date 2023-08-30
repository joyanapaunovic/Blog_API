using Blog.Application.DTO;
using Blog.Application.UseCases.DTO;
using Blog.DataAccess;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.Validators
{
    public class UpdateBlogPostValidator : AbstractValidator<UpdateBlogPostDTO>
    {
        private readonly BlogContext _context;
        public UpdateBlogPostValidator(BlogContext context)
        {
            _context = context;
            // BLOG POST TITLE
            RuleFor(x => x.BlogPostTitle).Cascade(CascadeMode.Stop)
                          .NotEmpty().WithMessage("Blog post title is required.")
                          .MinimumLength(5)
                                     .WithMessage("Minimum length for the blog post needs to be at least 5 characters.");
            // BLOG POST CONTENT
            RuleFor(x => x.BlogPostContent).Cascade(CascadeMode.Stop)
                          .NotEmpty().WithMessage("Blog post content is required.");
            // CATEGORIES
            RuleFor(x => x.Categories).Cascade(CascadeMode.Stop)
                                      .NotEmpty().WithMessage("Please enter the category ids" +
                                      " you would like this blog post to have.");
            // TAGS
            RuleFor(x => x.Tags).Cascade(CascadeMode.Stop)
                                      .NotEmpty().WithMessage("Please enter the tag ids" +
                                      " you would like this blog post to have or instead try to make " +
                                      "your own tag by entering the tag name.");
            // FILE PATH
            RuleFor(x => x.Files).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("File path cannot be empty. Please try again. ");
            var filePath_Regex = $"[^\\s]+(.*?)\\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$";
            RuleForEach(x => x.Files)
                            .Cascade(CascadeMode.Stop)
                            .NotEmpty().WithMessage("File paths cannot be empty.")
                            .Matches(filePath_Regex).WithMessage("File path can have an extension" +
                            " .jpg, .png, or .gif (possible even in uppercase letters)");

        }
    }
}
