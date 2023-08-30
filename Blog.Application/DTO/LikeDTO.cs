using Blog.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTO
{
    public class LikeDTO : BaseDTO
    {
       public int UserId { get; set; }
       public int? CommentId { get; set; }
       public int? BlogPostId { get; set; }
       public DateTime CreatedAt { get; set; }
    }
}
