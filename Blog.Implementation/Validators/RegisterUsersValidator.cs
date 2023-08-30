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
    public class RegisterUsersValidator : AbstractValidator<RegisterUserDTO>
    {
        private BlogContext _context;
        public RegisterUsersValidator(BlogContext context)
        {
            _context = context;
            // REGEX - first name/last name 
            var firstName_lastName_regex = @"^[A-Z][a-z]{2,}(\s[A-Z][a-z]{2,})?$";
            // REGEX - password
            var password_regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

            // FIRST NAME
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name cannot be null.")
                .MinimumLength(3).WithMessage("Minimum length for the first name has " +
                "to be at least 3 characters.")
                .Matches(firstName_lastName_regex)
                .WithMessage("First name is not in the right format. Please try again.");
            // LAST NAME
            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name cannot be null.")
                .MinimumLength(3).WithMessage("Minimum length for the last name has " +
                "to be at least 3 characters.")
                .Matches(firstName_lastName_regex)
                .WithMessage("Last name is not in the right format. Please try again.");
            // E-MAIL
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("E-mail is a required field. ")
                .EmailAddress().WithMessage("E-mail is not in the right format. ");
                //.Must(x => !_context.Users.Any(u => u.Email == x))
                //.WithMessage("E-mail address {PropertyValue} is already in use. Please try again.");
            // PASSWORD
            RuleFor(x => x.Password)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage("Password cannot be null.")
               .Matches(password_regex)
               .WithMessage("Password needs to contain minimum length of 8 characters. " +
               "It is required at least one uppercase letter, one lowercase letter, " +
               "one special character and one number.");
        }
    }
}
