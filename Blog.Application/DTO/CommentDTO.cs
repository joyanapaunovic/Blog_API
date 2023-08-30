using Blog.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTO
{
    public class CommentDTO : BaseDTO
    { 
        public string CommentContent { get; set; }
        public int UserId { get; set; }
        public int? BlogPostId { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentDTO> Children { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public ICollection<int> LikeIdsOnThisComment { get; set; }
    }
    // za potrebe prikaza u search-u prema blog post-u
   
}
