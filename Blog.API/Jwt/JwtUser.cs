using Blog.Domain;
using System.Collections.Generic;

namespace Blog.API.Core
{
    public class JwtUser : IApplicationUser
    {
        public string Identity { get; set; }

        public int Id { get; set; }
        public IEnumerable<int> UseCaseAllowedIds { get; set; } = new List<int>();
        public string Email { get; set; }
    }

    

}
