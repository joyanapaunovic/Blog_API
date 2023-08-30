using Blog.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTO
{
    public class UserDTO : BaseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public ICollection<int>? BlogPostsIdsRelatedToThisUser { get; set; }
        
    }
 

    public class ShowUsersToAdminDTO : UserDTO
    {
        public string Password { get; set; }
        public ICollection<int> UseCasesIdsAllowedForThisUser { get; set; }
    }
    public class CommonUserDTO : ShowUsersToAdminDTO
    {

    }
    public class UpdateUserDataDTO : RegisterUserDTO
    {
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int CurrentUserId { get; set; }
        public int UserWhoWasForwadedForUpdateId { get; set; }
    }

}
