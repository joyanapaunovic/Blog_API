using Blog.API.Core;
using Blog.Application.DTO;
using Blog.Application.DTO.Searches;
using Blog.Application.Logging;
using Blog.Application.UseCases.Commands.Users;
using Blog.Application.UseCases.Queries;
using Blog.Domain;
using Blog.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        // USE CASE HANDLER
        private UseCaseHandler _handler;
        private readonly IExceptionLogger _logger;

        public UsersController(UseCaseHandler handler, IExceptionLogger logger)
        {
            _handler = handler;
            _logger = logger;
        }
        // PRIKAZ SVIH KORISNIKA - ovo vide i admin i obicni useri - samo je prikaz prilagodjen ulozi
        // GET: api/<UserController>
        [HttpGet]
        public IActionResult Get([FromQuery] PageSearch search, 
                                 [FromServices] IGetUsersQuery query)
        {
            search.CurrentUser = _handler.HandleLoggingAndAuthorization(query, search);
            return Ok(_handler.HandleQuery(query, search));
        }
        // PODACI TRENUTNO ULOGOVANOG KORISNIKA / profil
        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromServices] IApplicationUser user)
        {
            return Ok(user);
        }
        // MODIFIKOVANJE PODATAKA - KORISNIK
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, 
                       [FromBody] UpdateUserDataDTO dto,
                       [FromServices] IUpdateUserDataCommand command)
        {
            var currentUser = _handler.HandleLoggingAndAuthorization(command, dto);
            dto.CurrentUserId = currentUser;
            dto.UserWhoWasForwadedForUpdateId = id;
            _handler.HandleCommand(command, dto);
            return StatusCode(200);
        }

      
    }
}
