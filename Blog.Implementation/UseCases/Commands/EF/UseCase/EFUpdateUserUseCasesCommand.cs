using Blog.Application.DTO;
using Blog.Application.UseCases.Commands.UseCase;
using Blog.DataAccess;
using Blog.Domain;
using Blog.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.UseCase
{
    public class EFUpdateUserUseCasesCommand : EFUseCase, IUpdateUserUseCasesCommand
    {
        private readonly UpdateUserUseCasesValidator _validator;
        public EFUpdateUserUseCasesCommand(BlogContext context, UpdateUserUseCasesValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 23;

        public string Name => "Update user allowed use cases";

        public string Description => "Updating user use cases using Entity Framework.";

        public void Execute(UpdateUserUseCasesDTO request)
        {
            _validator.ValidateAndThrow(request);

            var userUseCases = Context.UserUseCases
                                      .Where(x => x.UserId == request.UserId).ToList();
            Context.UserUseCases.RemoveRange(userUseCases);

            var allowedUseCases = request.AllowedUseCaseIds.Select(x => new UserUseCase
            {
                UserId = request.UserId.Value,
                UseCaseId = x

            });

            Context.AddRange(allowedUseCases);
            Context.SaveChanges();
            
          


        }
    }
}