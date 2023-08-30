using Blog.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTO
{
    public class TagDTO : BaseDTO
    {
        public string TagName { get; set; }
        public ICollection<int> RelatedBlogPostsIds { get; set; }
    }
    public class UpdateTagForBlogPostDTO : BaseDTO 
    {
        public string TagName { get; set; }
        public ICollection<int> NewTagIdsToAdd { get; set; }
    }
}
