
using Blog.Application.DTO;
using Blog.Application.Exceptions;
using Blog.Application.UseCases.Commands.Likes;
using Blog.DataAccess;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Implementation.UseCases.Commands.EF.Likes
{
    public class EFCreateLikesForCommentsOrBlogPostsCommand : EFUseCase, ICreateLikesForCommentsOrBlogPostsCommand
    {
        public EFCreateLikesForCommentsOrBlogPostsCommand(BlogContext context) : base(context)
        {
        }

        public int Id => 20;

        public string Name => "Create likes";

        public string Description => "Creating likes with Entity Framework.";

        public void Execute(LikeDTO request)
        {
            // Lajk zavisi gde je upucen od toga sta je poslato u body-ju - CommentId ili BlogPostId
            // oba svojstva su nalabilni int sto znaci da se uvek salje samo jedna vrednost
            // Ono sto se salje kroz body su jedno ova dva svojstva, ostalo je sve reseno u kodu
            // UserId uzimamo iz kontrolera,
            // FileId fiksan za sve jer je like isti uvek,
            // CreatedAt isto ovde pri kraju koda uzimamo
            // prakticno samo se salje CommentId ili BlogPostId
            // LAJK NA KOMENTAR
            if(request.CommentId != null)
            {
                var commentId = request.CommentId.Value;
                var ifUserAlreadyLiked = Context.CommentsLikes.Where(x => x.CommentId == commentId)
                                                            .Include(x => x.Like)
                                                            .Any(x => x.Like.UserId == request.UserId);
                var ifCommentExists = Context.Comments.Any(x => x.Id == commentId);
                if (!ifCommentExists)
                {
                    throw new InformationExceptionForUser("Dear user, it seems that the comment id you've entered doesn't exists. Please try again.");
                }
                if (ifUserAlreadyLiked)
                {
                    throw new InformationExceptionForUser("Dear user, you've already liked this comment.");
                }
                else
                {
                    var newLike = new Like
                    {
                        FileId = 3,
                        UserId = request.UserId
                    };
                    Context.Likes.Add(newLike);
                    Context.SaveChanges();
                    var likeId = newLike.Id; // novonapravljeni lajk
                    // veza sa komentarom
                    var newLikeOnComment = new CommentLike
                    {
                        CommentId = commentId,
                        LikeId = likeId
                    };
                    Context.CommentsLikes.Add(newLikeOnComment);
                    Context.SaveChanges();
                }
            }
            else if(request.BlogPostId != null)
            {
                var blogPostId = request.BlogPostId.Value;
                // treba da proverimo da li isti korisnik pokusava da lajkuje opet isti blog i treba ga spreciti
                // da se ne bi napravila potpuno ista veza
                var ifUserAlreadyLiked = Context.BlogPostsLikes.Where(x => x.BlogPostId == blogPostId)
                                                               .Include(x => x.Like)
                                                               .Any(x => x.Like.UserId == request.UserId);

                var ifBlogPostExists = Context.BlogPosts.Any(x => x.Id == blogPostId);
                if (!ifBlogPostExists)
                {
                    throw new InformationExceptionForUser("Dear user, it seems that the blog post id you've entered doesn't exists. Please try again.");
                }
                if (ifUserAlreadyLiked)
                {
                    throw new InformationExceptionForUser("Dear user, you've already liked this blog post.");
                }                    
                else
                {
                    var newLike = new Like
                    {
                        FileId = 3,
                        UserId = request.UserId
                    };
                    Context.Likes.Add(newLike);
                    Context.SaveChanges();
                    var likeId = newLike.Id; // novonapravljeni lajk
                    // veza sa blogom
                    var newLikeOnBlogPost = new BlogPostLike
                    {
                        BlogPostId = blogPostId,
                        LikeId = likeId
                    };
                    Context.BlogPostsLikes.Add(newLikeOnBlogPost);
                    Context.SaveChanges();
                }
            }
            else if(request.CommentId == null && request.BlogPostId == null)
            {
                throw new InformationExceptionForUser("Dear user, please let us know what would you love to like, blog post or some " +
                "of the comments below and please specify the right id. (send us the value of BlogPostId or CommentId)");
            }
        }
    }
}
