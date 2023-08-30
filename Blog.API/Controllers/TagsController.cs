using Blog.Application.DTO;
using Blog.Application.DTO.Searches;
using Blog.Application.Logging;
using Blog.Application.UseCases.Commands.Tags;
using Blog.Application.UseCases.Queries;
using Blog.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagsController : ControllerBase
    {

        // USE CASE HANDLER
        private UseCaseHandler _handler;
        private readonly IExceptionLogger _logger;

        public TagsController(UseCaseHandler handler, IExceptionLogger logger)
        {
            _handler = handler;
            _logger = logger;
        }
        // PRETRAGA TAGOVA
        [HttpGet]
        public IActionResult Get([FromQuery] PageSearch search,
                                 [FromServices] IGetTagsQuery query)
        {
            return Ok(_handler.HandleQuery(query, search));
        }
        // KREIRANJE TAGA
        [HttpPost]
        public IActionResult Post([FromBody] TagDTO dto, 
                                  [FromServices] ICreateTagCommand command)
        {
            _handler.HandleCommand(command, dto);
            return StatusCode(201);
        }

        // UPDATE TAGA
        [HttpPut("{id}")]
        public IActionResult Put(int id, 
                       [FromBody] TagDTO dto, 
                       [FromServices] IUpdateTagCommand command)
        {
            dto.Id = id;
            _handler.HandleCommand(command, dto);
            return StatusCode(200);
        }
        // BRISANJE TAGOVA
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, 
                                   [FromServices] IDeleteTagCommand command)
        {
            _handler.HandleCommand(command, id);
            return NoContent();
        }
    }
}
