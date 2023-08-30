
using Blog.Application.DTO;
using Blog.DataAccess;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.Validators
{
    public class CreateTagValidator : AbstractValidator<TagDTO>
    {
        private BlogContext _context;
        public CreateTagValidator(BlogContext context) 
        {
            _context = context;

            RuleFor(x => x.TagName)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Tag name cannot be null.")
                .MinimumLength(3).WithMessage("Tag name has to have at least 3 characters.")
                .Must(IsTheTagAlreadyInUse).WithMessage("The tag {PropertyValue} is already in use.");
        }

        public bool IsTheTagAlreadyInUse(string tagName)
        {
            var query = _context.Tags.Any(x => x.TagName == tagName);
            return !query;
        }
    }
}
