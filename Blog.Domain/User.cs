using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public HashSet<Comment> Comments { get; set; } = new HashSet<Comment>();
        public HashSet<Like> Likes { get; set; } = new HashSet<Like>();
        public HashSet<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();
        public IEnumerable<UserUseCase> UseCases { get; set; }


    }
}
