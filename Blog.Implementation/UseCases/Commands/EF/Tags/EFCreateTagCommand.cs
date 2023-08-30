using Blog.Application.DTO;
using Blog.Application.UseCases.Commands.Tags;
using Blog.DataAccess;
using Blog.Domain;
using Blog.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Tags
{
    public class EFCreateTagCommand : EFUseCase, ICreateTagCommand
    {
        private readonly CreateTagValidator _validator;
        public EFCreateTagCommand(BlogContext context, CreateTagValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 12;

        public string Name => "Create tag";

        public string Description => "Creating tags using Entity Framework.";

        public void Execute(TagDTO dto)
        {
            // provera
            _validator.ValidateAndThrow(dto);

            var newTag = new Tag
            {
                TagName = dto.TagName
            };

            Context.Add(newTag);
            Context.SaveChanges();
        }
    }
}
