using Blog.Application.UseCases.Commands.Categories;
using Blog.Application.UseCases.DTO;
using Blog.DataAccess;
using Blog.Domain;
using Blog.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Categories
{
    public class EFCreateCategoryCommand : EFUseCase, ICreateCategoryCommand
    {

        private CreateCategoryValidator _validator;
        public EFCreateCategoryCommand(BlogContext context, CreateCategoryValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 2;

        public string Name => "Create category";

        public string Description => "Create category using Entity Framework.";

        public void Execute(CreateCategoryDTO request)
        {
            // VALIDATE
            _validator.ValidateAndThrow(request);

            var newCategory = new Category
            {
                Title = request.Title
            };

            // ADD AND SAVE
            Context.Categories.Add(newCategory);

            Context.SaveChanges();

        }
    }
}
