using Blog.Application.DTO;
using Blog.Application.Logging;
using Blog.Application.UseCases.Commands.Comments;
using Blog.Application.UseCases.Queries;
using Blog.DataAccess;
using Blog.Domain;
using Blog.Implementation;
using Blog.Implementation.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly BlogContext _context;
        private UseCaseHandler _handler;
        private readonly IExceptionLogger _logger;
        public CommentsController(BlogContext context, UseCaseHandler handler, IExceptionLogger logger)
        {
            _context = context;
            _handler = handler;
            _logger = logger;
        }
        // PRIKAZ KOMENTARA I NJEGOVIH CHILD-OVA ZAJEDNO SA LAJKOVIMA KOMENTARA
        [HttpGet]
        public IActionResult Get([FromQuery] int blogPostId,
                                 [FromServices] IGetCommentsQuery query)
        {
            return Ok(_handler.HandleQuery(query, blogPostId));
        }

        // DODAVANJE KOMENTARA
        [HttpPost]
        public IActionResult Post([FromBody] CommentDTO dto, 
                                  [FromServices] CreateCommentValidator commentValidator,
                                  [FromServices] ICreateCommentCommand command)
        {
            dto.UserId = _handler.HandleLoggingAndAuthorization(command, dto);
            _handler.HandleCommand(command, dto);
            return StatusCode(201);
        }

        // MODIFIKOVANJE KOMENTARA
        [HttpPut("{id}")]
        public IActionResult Put(int id, 
                        [FromBody] CommentDTO dto,
                        [FromServices] IUpdateCommentCommand command)
        {
            dto.Id = id;
            _handler.HandleCommand(command, dto);
            return StatusCode(200);
        }
        // BRISANJE KOMENTARA
        [HttpDelete("{id}")]
        public IActionResult Delete(int id,
                                    UserDTO dto,
                                    [FromServices] IDeleteCommentCommand command)
        {
            var userId = _handler.HandleLoggingAndAuthorization(command, id);
            dto.Id = userId;
            _handler.HandleCommandWithSecondParameter<int, UserDTO>(command, id, dto);
            return NoContent();
        }
    }
}
