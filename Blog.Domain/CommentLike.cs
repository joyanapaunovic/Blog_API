using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class CommentLike
    {
        public Comment Comment { get; set; }
        public Like Like { get; set; }
        public int CommentId { get; set; }
        public int LikeId { get; set; }
    }
}
