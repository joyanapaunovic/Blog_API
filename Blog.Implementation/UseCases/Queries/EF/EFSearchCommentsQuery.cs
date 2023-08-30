using Blog.Application.DTO;
using Blog.Application.Exceptions;
using Blog.Application.UseCases.DTO;
using Blog.Application.UseCases.Queries;
using Blog.DataAccess;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blog.Implementation.UseCases.Queries.EF
{
    public class EFSearchCommentsQuery : EFUseCase, IGetCommentsQuery
    {
        public EFSearchCommentsQuery(BlogContext context) : base(context)
        {
        }

        public int Id => 21;

        public string Name => "Search comments";

        public string Description => "Searching comments using Entity framework.";
        // prilikom pretrazivanja komentara prosledjuje se id blog post-a ali kroz query string
        public BlogPostWithOnlyIdAndParentCommentWithChildrenDTO Execute(int blogPostId)
        {
            if(blogPostId == 0)
            {
                throw new InformationExceptionForUser("Dear user, this is search by blog post id, so you need to add a query string in your url with the required value blogPostId. ");
            }
            // komentari sa lajkovima
             var query = Context.Comments.Where(x => x.BlogPostId == blogPostId)
                              .Include(x => x.Likes).ThenInclude(x => x.Like)
                              .ToList();
        
            // prikaz strukture komentara
            var result = new BlogPostWithOnlyIdAndParentCommentWithChildrenDTO
            {
                BlogPostId = blogPostId,
                MainCommentWithChildren = BuildCommentHierarchy(query, null)
            };
            return result;
        }

        

        // REKURZIVNA FUNKCIJA ZA ORGANIZACIJU KOMENTARA NA GRANANJE 
        private List<CommentDTO> BuildCommentHierarchy(List<Comment> comments, int? parentCommentId)
        {
            var allComments = new List<CommentDTO>();

            foreach (var comment in comments.Where(x => x.ParentCommentId == parentCommentId))
            {
                var commentDTO = new CommentDTO
                {
                    Id = comment.Id,
                    CommentContent = comment.CommentContent,
                    UserId = comment.UserId,
                    BlogPostId = comment.BlogPostId,
                    CreatedAt = comment.CreatedAt,
                    ParentCommentId = parentCommentId,
                    LikeIdsOnThisComment = comment.Likes
                    .Where(x=> x.CommentId == comment.Id)
                    .Select(x => x.LikeId).ToList()
                };

                commentDTO.Children = BuildCommentHierarchy(comments, comment.Id);
                allComments.Add(commentDTO);
            }

            return allComments;
        }

    }
}
