using Blog.Application.UseCases.DTO;
using Blog.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blog.Implementation.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDTO>
    {
        private BlogContext _context;
        public CreateCategoryValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Category title cannot be null.")
            .MinimumLength(3).WithMessage("Minimum length for the category has " +
            "to be at least 3 characters.")
            .Must(CategoryNotInUse).WithMessage("Category {PropertyValue} already exists.");
           
        }

        private bool CategoryNotInUse(string name)
        {
            var if_exists = _context.Categories.Any(x => x.Title == name);

            return !if_exists;
        }

    }
}
