using Blog.Application.UseCases.Commands.Categories;
using Blog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Categories
{
    public class EFDeleteCategoryCommand : EFUseCase, IDeleteCategoryCommand
    {
        public EFDeleteCategoryCommand(BlogContext context) : base(context)
        {
        }

        public int Id => 7;

        public string Name => "Delete category";

        public string Description => "Deleting category using Entity Framework. ";

        public void Execute(int request)
        {
            var categoryWithForwadedId = Context.Categories.Find(request);

            var categories = Context.BlogPostsCategories.Where(x => x.CategoryId == request);
            Context.BlogPostsCategories.RemoveRange(categories);
            if (categoryWithForwadedId != null)
            {
                Context.Categories.Remove(categoryWithForwadedId);
            }
            Context.SaveChanges();


        }
    }
}
