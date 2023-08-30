using Blog.Application.UseCases.DTO;
using Blog.Implementation;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Blog.API.Extensions;
using Blog.Application.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Blog.Domain;
using Blog.Application.UseCases.Commands.Users;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegisterController : ControllerBase
    { 
        private UseCaseHandler _handler;
        private readonly IExceptionLogger _logger;
        public RegisterController(UseCaseHandler handler, IExceptionLogger logger)
        {
            _handler = handler;
            _logger = logger;
        }
        // R E G I S T R A C I J A
        // POST api/<RegisterController>
        [HttpPost]
        public IActionResult Post([FromBody] RegisterUserDTO dto,
                                  [FromServices] IRegisterUserCommand command)
        {
                _handler.HandleCommand(command, dto);
                return StatusCode(201); 
        }
    }
}
