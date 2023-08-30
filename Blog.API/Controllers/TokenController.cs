using Blog.Api.Core;
using Blog.Implementation.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JwtManager _manager;
        public TokenController(JwtManager manager)
        {
            _manager = manager;
        }
        // POST api/<TokenController>
        public class TokenRequest
        {
            public string Password { get; set; }
            public string Email { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] TokenRequest token)
        {
            // generisanje tokena
            try
            {
                var tokenObj = _manager.MakeToken(token.Email, token.Password);
                return Ok(new { tokenObj });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch(System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
