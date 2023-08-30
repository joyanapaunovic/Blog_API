using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class Comment : Entity
    {
        public string CommentContent { get; set; }
        public int UserId { get; set; }
        public int BlogPostId { get; set; }
        public int? ParentCommentId { get; set; }

        public User User { get; set; }
        public BlogPost BlogPost { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Children { get; set; } = new List<Comment>();
        public ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();
       
    }
}
