using Blog.Application.UseCases.Commands.Categories;
using Blog.Application.UseCases.DTO;
using Blog.DataAccess;
using Blog.Implementation.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Categories
{
    public class EFUpdateCategoryCommand : EFUseCase, IUpdateCategoryCommand
    {
        private CreateCategoryValidator _validator;
        public EFUpdateCategoryCommand(BlogContext context, CreateCategoryValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 11;

        public string Name => "Update a category";

        public string Description => "Updating categories using Entity Framework.";

        public void Execute(CreateCategoryDTO request)
        {
            _validator.ValidateAndThrow(request);

            var categoryForUpdate = Context.Categories.Find(request.Id);

            categoryForUpdate.Title = request.Title;
            categoryForUpdate.UpdatedAt = DateTime.Now;
            categoryForUpdate.UpdatedBy = "admin@gmail.com";

            Context.SaveChanges();
        }
    }
}
