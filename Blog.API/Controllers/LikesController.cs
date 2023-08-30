using Blog.Application.DTO;
using Blog.Application.Logging;
using Blog.Application.UseCases.Commands.Comments;
using Blog.Application.UseCases.Commands.Likes;
using Blog.DataAccess;
using Blog.Implementation;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        // GET: api/<LikesController>
        private readonly BlogContext _context;
        private UseCaseHandler _handler;
        private readonly IExceptionLogger _logger;

        public LikesController(BlogContext context, UseCaseHandler handler, IExceptionLogger logger)
        {
            _context = context;
            _handler = handler;
            _logger = logger;
        }

        

        

        // KREIRANJE LAJKA (NA KOMENTAR ILI NA BLOG POST)
        [HttpPost]
        public IActionResult Post([FromBody] LikeDTO dto,
                         [FromServices] ICreateLikesForCommentsOrBlogPostsCommand command)
        {
            dto.UserId = _handler.HandleLoggingAndAuthorization(command, dto);
            _handler.HandleCommand(command, dto);
            return StatusCode(201);
        }

        // BRISANJE LAJKA NA KOMENTAR
        // DELETE api/<LikesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id,
                                    [FromServices] IDeleteLikeForCommentsCommand command)
        {
            _handler.HandleCommand(command, id);
            return NoContent();
        }
    }
}
