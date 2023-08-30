using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class Like : Entity
    {
        public int FileId { get; set; }
        public File LikeIcon { get; set; }
        // who liked
        public User User { get; set; }
        public int UserId { get; set; }
        // likes
        public HashSet<BlogPostLike> BlogPosts { get; set; } = new HashSet<BlogPostLike>();
        public ICollection<CommentLike> Comments { get; set; } = new List<CommentLike>();
    }
}
