using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTO
{
    public class UpdateUserUseCasesDTO
    {
        public int? UserId { get; set; }
        public ICollection<int> AllowedUseCaseIds { get; set; }
    }
}
