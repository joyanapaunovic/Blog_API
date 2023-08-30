using Blog.Application.UseCases.Commands.Tags;
using Blog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Tags
{
    public class EFDeleteTagCommand : EFUseCase, IDeleteTagCommand
    {
        public EFDeleteTagCommand(BlogContext context) : base(context)
        {
        }

        public int Id => 9;

        public string Name => "Delete a tag";

        public string Description => "Deleting tags using Entity Framework.";

        public void Execute(int request)
        {
            var tagId = Context.Tags.Find(request);

            var tags = Context.BlogPostsTags.Where(x => x.TagId == request);
            Context.BlogPostsTags.RemoveRange(tags);
            if (tagId != null)
            {
                Context.Tags.Remove(tagId);
            }
            Context.SaveChanges();
        }
    }
}
