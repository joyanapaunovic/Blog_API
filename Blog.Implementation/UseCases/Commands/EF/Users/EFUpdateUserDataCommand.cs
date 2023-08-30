using Blog.Application.DTO;
using Blog.Application.Exceptions;
using Blog.Application.UseCases.Commands.Users;
using Blog.DataAccess;
using Blog.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Users
{
    public class EFUpdateUserDataCommand : EFUseCase, IUpdateUserDataCommand
    {
        private readonly RegisterUsersValidator _updateUsersValidator;

        public EFUpdateUserDataCommand(BlogContext context,
                                       RegisterUsersValidator validator) : base(context)
        {
            _updateUsersValidator = validator;
        }

        public int Id => 15;

        public string Name => "Update user data";

        public string Description => "Updating user data using Entity Framework.";

        public void Execute(UpdateUserDataDTO request)
        {
            if (request.UserWhoWasForwadedForUpdateId != request.CurrentUserId)
            {
                var user = Context.Users.Where(x => x.Id == request.CurrentUserId)
                                        .Select(x => x.Email);
                foreach (var u in user)
                {
                    throw new ForbiddenUseCaseExecutionException(u, Name);
                }

            }
            else
            {
                var fetchThatUserData = Context.Users.Find(request.UserWhoWasForwadedForUpdateId);
                /* FIRST NAME */
                _updateUsersValidator.ValidateAndThrow(request);
                fetchThatUserData.FirstName = request.FirstName;
                /* LAST NAME */
                fetchThatUserData.LastName = request.LastName;
                /* EMAIL */
                fetchThatUserData.Email = request.Email;
                /* PASSWORD */
                var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                fetchThatUserData.Password = hash;
                /* UPDATED AT, UPDATED BY */
                fetchThatUserData.UpdatedBy = fetchThatUserData.Email;
                fetchThatUserData.UpdatedAt = DateTime.UtcNow;
                // SAVE
                Context.SaveChanges();
            }
        }
    }
}
