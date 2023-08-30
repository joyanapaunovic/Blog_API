using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class File : Entity
    {
        public string Path { get; set; }
        public HashSet<BlogPostFile> BlogPosts { get; set; } = new HashSet<BlogPostFile>();
    }
}
