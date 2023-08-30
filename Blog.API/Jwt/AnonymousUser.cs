using Blog.Domain;
using System.Collections.Generic;

namespace Blog.API.Core
{
    public class AnonymousUser : IApplicationUser
    {
        public string Identity => "Anonymous";

        public int Id => 0;

        public IEnumerable<int> UseCaseAllowedIds => new List<int> { 3 };

        public string Email => "anonymous@gmail.com";
    }
}
