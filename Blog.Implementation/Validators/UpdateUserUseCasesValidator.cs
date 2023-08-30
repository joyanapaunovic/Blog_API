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
    public class UpdateUserUseCasesValidator : AbstractValidator<UpdateUserUseCasesDTO>
    {
        private readonly BlogContext _context;
        public UpdateUserUseCasesValidator(BlogContext context) 
        {
            _context = context;
            // USER ID
            RuleFor(x => x.UserId).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("User is a required field. ")
                                  .Must(x => context.Users.Any(user => user.Id == x))
                                  .WithMessage("The user doesn't exists in our database. Please try again.");
            // USE CASE ID
            RuleFor(x => x.AllowedUseCaseIds).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Use cases are a required field. ")
                                             .Must(x => x.Count() == x.Distinct().Count())
                                             .WithMessage("Duplicate values are not allowed. ");
        }
    }
}
