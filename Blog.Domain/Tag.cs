using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class Tag : Entity
    {
        public string TagName { get; set; }
        public HashSet<BlogPostTag> BlogPosts { get; set; } = new HashSet<BlogPostTag>();
    }
}
