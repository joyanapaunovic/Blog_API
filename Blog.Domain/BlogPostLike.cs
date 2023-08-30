using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class BlogPostLike
    {
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int LikeId { get; set; }
        public Like Like { get; set; }
    }
}
