using Blog.Application.UseCases.DTO;
using Blog.DataAccess;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.Validators
{
    public class CreateBlogPostValidator : AbstractValidator<CreateBlogPostDTO>
    {
        private BlogContext _context;

        public CreateBlogPostValidator(BlogContext context)
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
                         .NotEmpty().WithMessage("Categories are required.");
            RuleForEach(x => x.Categories)
                              .Cascade(CascadeMode.Stop)
                              .NotEmpty().WithMessage("Category is required.")
                              .Must(x => context.Categories.Any(c => c.Id == x))
                              .WithMessage($"The category id you have entered doesn't exists " +
                              $"in the database.");
            RuleFor(x => x.Tags).Cascade(CascadeMode.Stop)
                         .NotEmpty().WithMessage("Tags are required.");
            // TAGS
            RuleForEach(x => x.Tags).Cascade(CascadeMode.Stop)
                              .NotEmpty().WithMessage("Blog post tag is required.")
                              .Must(x => context.Tags.Any(c => c.Id == x))
                              .WithMessage($"The tag id you have entered doesn't exists in the database.");


            // FILES - FILE PATH
            var filePath_Regex = $"[^\\s]+(.*?)\\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$";
            RuleFor(x => x.Files).Cascade(CascadeMode.Stop)
                         .NotEmpty().WithMessage("Files are required.");
            RuleForEach(x => x.Files)
                              .Cascade(CascadeMode.Stop)
                              .NotEmpty().WithMessage("File paths cannot be empty.")
                              .Matches(filePath_Regex).WithMessage("File path can have an extension" +
                              " .jpg, .png, or .gif (possible even in uppercase letters)");



        }

    }
}
