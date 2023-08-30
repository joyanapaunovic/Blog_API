using Blog.Application.DTO;
using Blog.Application.UseCases.Commands.Comments;
using Blog.Application.UseCases.Commands.Likes;
using Blog.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Likes
{
    public class EFDeleteLikeForCommentCommand : EFUseCase, IDeleteLikeForCommentsCommand
    {
        public EFDeleteLikeForCommentCommand(BlogContext context) : base(context)
        {
        }

        public int Id => 22;

        public string Name => "Unlike a comment";

        public string Description => "Deleting likes on comments using Entity Framework.";


        public void Execute(int request)
        {
            var likeId = Context.Likes.Find(request);

            var likes = Context.CommentsLikes.Where(x => x.LikeId == request);
            Context.CommentsLikes.RemoveRange(likes);
            if (likeId != null)
            {
                Context.Likes.Remove(likeId);
            }
            Context.SaveChanges();
        }
    }
}
