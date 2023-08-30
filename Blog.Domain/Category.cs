using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class Category : Entity
    {
        public string Title { get; set; }
        public HashSet<BlogPostCategory> BlogPosts { get; set; } = new HashSet<BlogPostCategory>();
        
    }
}
