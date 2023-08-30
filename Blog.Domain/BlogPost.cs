using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class BlogPost : Entity
    {
        public string BlogPostTitle { get; set; }
        public string BlogPostContent { get; set; }
        // author
        public int UserId { get; set; }
        public User User { get; set; }
        // kolekcija tipa BlogPostCategory, kolekcija tipa BlogPostTag
        public HashSet<BlogPostCategory> Categories { get; set; } = new HashSet<BlogPostCategory>();
        public HashSet<BlogPostTag> Tags { get; set; } = new HashSet<BlogPostTag>();
        public HashSet<BlogPostFile> Files { get; set; } = new HashSet<BlogPostFile>();
        public HashSet<BlogPostLike> Likes { get; set; } = new HashSet<BlogPostLike>();
        public HashSet<Comment> Comments { get; set; } = new HashSet<Comment>();
        
    }
}
