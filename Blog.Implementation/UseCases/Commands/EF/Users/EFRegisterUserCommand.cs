using BCrypt.Net;
using Blog.DataAccess;
using Blog.Domain;
using Blog.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Application.UseCases.DTO;
using Blog.Application.UseCases.Commands.Users;
using Blog.DataAccess.Migrations;

namespace Blog.Implementation.UseCases.Commands.EF.Users
{
    public class EFRegisterUserCommand : EFUseCase, IRegisterUserCommand
    {
        private readonly RegisterUsersValidator _validator;
        public EFRegisterUserCommand(BlogContext context, RegisterUsersValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 3;

        public string Name => "Registration - new users";

        public string Description => "Registration using Entity Framework.";

        public void Execute(RegisterUserDTO dto)
        {
            // validacija, registracija/upis u bazu
            _validator.ValidateAndThrow(dto);
            // string to hash !!!!
            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            // UPIS U BAZU - INSERT
            var newUser = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = hash
            }; 
            Context.Users.Add(newUser);
            Context.SaveChanges();
            var useCaseDefaultForUser = new List<int> { 1, 4, 5, 6, 8, 9, 10, 12, 14, 15, 16, 17, 18, 19, 20, 21, 22 };
            // u trenutku registrovanja korisnik dobija use case-ove DEFAULT
            // kasnije to moze da menja admin
            foreach (var useCaseId in useCaseDefaultForUser)
            {
                var id = useCaseId;
                var userUseCase = new UserUseCase
                {
                    UseCaseId = id,
                    UserId = newUser.Id
                };
                Context.UserUseCases.Add(userUseCase);
            }

            Context.SaveChanges();

            // ****slanje mejla korisniku?


        }


    }
}
