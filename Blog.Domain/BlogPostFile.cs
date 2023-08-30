using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class BlogPostFile
    {
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int FileId { get; set; }
        public File File { get; set; }
    }
}
