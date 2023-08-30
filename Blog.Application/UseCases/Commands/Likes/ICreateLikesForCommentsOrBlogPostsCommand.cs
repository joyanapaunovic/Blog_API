using Blog.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.UseCases.Commands.Likes
{
    public interface ICreateLikesForCommentsOrBlogPostsCommand : IUseCase, ICommand<LikeDTO>
    {
    }
}
