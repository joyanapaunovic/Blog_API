using Blog.Application.DTO;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.DTO
{
    public class BlogPostDTO : BaseDTO
    {
        public string BlogPostTitle { get; set; }
        public string BlogPostContent { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<CategoryDTO> Categories { get; set; }
        public virtual ICollection<TagDTO> Tags { get; set; }
        public virtual ICollection<FileDTO> Files { get; set; }
        public virtual ICollection<CommentDTO> Comments { get; set; }
        public virtual ICollection<int> IdLikesOnThisBlogPost { get; set; }
    }
    public class CreateBlogPostDTO : BaseDTO
    {
        public string BlogPostTitle { get; set; }
        public string BlogPostContent { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<int> Categories { get; set; }
        public virtual ICollection<int> Tags { get; set; }
        public virtual ICollection<string> Files { get; set; }

    }
    public class UpdateBlogPostDTO : BaseDTO
    {
        public string BlogPostTitle { get; set; }
        public string BlogPostContent { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<UpdateCategoryDTO> Categories { get; set; }
        public virtual ICollection<UpdateTagForBlogPostDTO> Tags { get; set; }
        public virtual ICollection<string> Files { get; set; }
        
    }
    public class BlogPostWithOnlyIdAndParentCommentWithChildrenDTO
    {
        public int BlogPostId { get; set; }
        public List<CommentDTO> MainCommentWithChildren { get; set; }
    }
}
