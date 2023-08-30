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
    public class UpdateTagValidator : AbstractValidator<UpdateTagForBlogPostDTO>
    {
        private BlogContext _context;
        public UpdateTagValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.TagName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tag name cannot be null.")
                .MinimumLength(3).WithMessage("Tag name has to have at least 3 characters.");
        }
    }
}
