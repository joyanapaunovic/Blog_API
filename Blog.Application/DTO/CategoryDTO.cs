using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.DTO
{
    // QUERY - SEARCH CATEGORIES - GET
    public class CategoryDTO : BaseDTO
    {
        public string Title { get; set; }
        public ICollection<int> BlogPostsIdsRelatedWithThisCategory { get; set; }
    }
    
    public class CreateCategoryDTO : BaseDTO
    {
        public string Title { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public ICollection<int> NewCategoryIdsToAdd { get; set; }
    }
   

}
