using Blog.API.Extensions;
using Blog.Application.DTO;
using Blog.Application.DTO.Searches;
using Blog.Application.Exceptions;
using Blog.Application.Logging;
using Blog.Application.UseCases;
using Blog.Application.UseCases.Commands.Blog_posts;
using Blog.Application.UseCases.DTO;
using Blog.Application.UseCases.Queries;
using Blog.Domain;
using Blog.Implementation;
using Blog.Implementation.UseCases.Commands.EF;
using Blog.Implementation.UseCases.Queries.EF;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Management.Automation.Runspaces;
using System.Runtime.ExceptionServices;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogPostsController : ControllerBase
    {
        // USE CASE HANDLER
        private UseCaseHandler _handler;
        private readonly IExceptionLogger _logger;

        public BlogPostsController(UseCaseHandler handler, IExceptionLogger logger)
        {
            _handler = handler;
            _logger = logger;
        }

        // PRETRAGA
        [HttpGet]
        public IActionResult Get([FromQuery] PageSearch search,
                                 [FromServices] IGetBlogPostsQuery query)
        {
            var userId = _handler.HandleLoggingAndAuthorization(query, search);
            search.CurrentUser = userId;
            return Ok(_handler.HandleQuery(query, search));
        }

        // PRIKAZ JEDNOG BLOG POST-A PREMA PROSLEDJENOM ID-u
        [HttpGet("{id}")]
        public IActionResult Get(int id, 
                                [FromBody] BlogPostDTO dto,
                                [FromServices] IGetOneBlogPostQuery query)
        {
            var data = _handler.HandleQuery(query, id);
            return Ok(data);
        }

        // KREIRANJE BLOG POST-A
        [HttpPost]
        public IActionResult Post([FromBody] CreateBlogPostDTO dto,
                                  [FromServices] ICreateBlogPostCommand command)
        {
            // dohvatanje ulogovanog korisnika
           var userId = _handler.HandleLoggingAndAuthorization(command, dto);
            dto.UserId = userId;
            _handler.HandleCommand(command, dto);
            return StatusCode(201);
        }
        // UPDATE BLOG POST-A
        [HttpPut("{id}")]
        public IActionResult Put(int id, 
                       [FromBody] UpdateBlogPostDTO dto,
                       [FromServices] IUpdateBlogPostCommand command)
        {
            dto.Id = id;
            var userId = _handler.HandleLoggingAndAuthorization(command, dto);
            dto.UserId = userId;
            _handler.HandleCommand(command, dto);
            return StatusCode(200);
        }
        // BRISANJE BLOG POST-A
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, 
                           UserDTO user,
                           [FromServices] IDeleteBlogPostCommand command)
        {
            var userId = _handler.HandleLoggingAndAuthorization(command, id);
            user.Id = userId;
            _handler.HandleCommandWithSecondParameter<int, UserDTO>(command, id, user);
            return NoContent();
        }
        
    }
}
