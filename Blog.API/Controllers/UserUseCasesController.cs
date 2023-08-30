using Blog.Application.DTO;
using Blog.Application.UseCases.Commands.UseCase;
using Blog.Implementation;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUseCasesController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public UserUseCasesController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        
        [HttpPut]
        public IActionResult Put(
                                [FromBody] UpdateUserUseCasesDTO dto,
                                [FromServices] IUpdateUserUseCasesCommand command)
        {
            _handler.HandleCommand(command, dto);
            return NoContent();
        }
    }
}
