using Blog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Application.UseCases.Commands.Comments;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Exceptions;
using Blog.Application.DTO;

namespace Blog.Implementation.UseCases.Commands.EF.Comments
{
    public class EFDeleteCommentCommand : EFUseCase, IDeleteCommentCommand
    {
        public EFDeleteCommentCommand(BlogContext context) : base(context)
        {
        }

        public int Id => 19;

        public string Name => "Delete comment(s)";

        public string Description => "Deleting comments using Entity Framework.";

       

        public void Execute(int request, UserDTO dto)
        {
            void RecursiveDelete(int commentId)
            {
                //Console.WriteLine(dto.Id);
                //Console.WriteLine(commentId);
                //var isTheUserOwnerOfTheComment = Context.Comments
                //                                        .Any(x => x.UserId == dto.Id && x.Id == commentId);
                //if (!isTheUserOwnerOfTheComment)
                //{
                //    throw new InformationExceptionForUser("Dear user, it seems like you're trying to delete a comment that's not yours.");
                //}
                //var allTheComments = Context.Comments.Where(x => x.Id == commentId)
                //                                     .Select(x => x.BlogPostId).ToList();
                //foreach(var v in allTheComments)
                //{
                //    var isTheUserOwnerOfTheBlogPost = Context.BlogPosts
                //                                        .Any(x => x.Id == v && x.UserId == dto.Id);
                //    if (!isTheUserOwnerOfTheBlogPost)
                //    {
                //        throw new InformationExceptionForUser("Dear user, you need to either be an owner of the comment or the owner of the blog post.");
                //    }
                //}


                // prvo brisemo lajkove na komentar ako postoje
                var commentLikes = Context.CommentsLikes.Any(x => x.CommentId == commentId);
                if (commentLikes)
                {
                    var likes = Context.CommentsLikes.Where(x => x.CommentId == commentId);
                    Context.CommentsLikes.RemoveRange(likes);
                    Context.SaveChanges();
                }

                /**
                * brisemo svako grananje komentara,
                odnosno ako prosledjeni id u url-u ima funkciju parent comment-a negde,
                a mesto gde je on roditelj, id child komentara moze da
                ima funkciju roditelja nekom drugom komentaru;
                => brisemo id tih child komentara sa ciljem da obrisemo prvo gde oni imaju funkciju roditelja,
                a potom na samom kraju brisemo child-ove gde je prosledjeni id parent, a na kraju i samog parent-a
                **/
                // CHILD komentari 
                var childComments = Context.Comments.Where(x => x.ParentCommentId == commentId).ToList();
                foreach (var childComment in childComments)
                {
                    var child = childComment.Id;
                    RecursiveDelete(child);
                }
                // MAIN *PARENT* komentar 
                var commentToDelete = Context.Comments.Find(commentId);
                if (commentToDelete != null)
                {
                    Context.Comments.Remove(commentToDelete);
                }
                Context.SaveChanges();
            }

            // -> poziv rekurzivne funkcije sa parametrom koji prosledjujemo pri pristupanju ruti .../comments/{id}
            RecursiveDelete(request);
            
        }

        }
}

