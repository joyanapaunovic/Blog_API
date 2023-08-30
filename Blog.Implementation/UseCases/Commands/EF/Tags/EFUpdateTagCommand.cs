using Blog.Application.DTO;
using Blog.Application.UseCases.Commands.Tags;
using Blog.DataAccess;
using Blog.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Tags
{
    public class EFUpdateTagCommand : EFUseCase, IUpdateTagCommand
    {
        private readonly CreateTagValidator _validator;
        public EFUpdateTagCommand(BlogContext context, CreateTagValidator validator) : base(context)
        {
            _validator = validator;
        }

        public int Id => 13;

        public string Name => "Update tag";

        public string Description => "Updating tags using Entity Framework.";

        public void Execute(TagDTO request)
        {
            _validator.ValidateAndThrow(request);

            var tagForUpdate = Context.Tags.Find(request.Id);

            tagForUpdate.TagName = request.TagName;
            tagForUpdate.UpdatedAt = DateTime.Now;
            tagForUpdate.UpdatedBy = "admin@gmail.com";


            Context.SaveChanges();
        }
    }
}
