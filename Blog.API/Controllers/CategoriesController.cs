using Blog.API.Extensions;
using Blog.Application.DTO.Searches;
using Blog.Application.Exceptions;
using Blog.Application.Logging;
using Blog.Application.UseCases.Commands.Categories;
using Blog.Application.UseCases.DTO;
using Blog.Application.UseCases.Queries;
using Blog.Implementation;
using Blog.Implementation.UseCases.Commands.EF;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.ExceptionServices;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        // USE CASE HANDLER
        private UseCaseHandler _handler;
        private readonly IExceptionLogger _logger;

        public CategoriesController(UseCaseHandler handler, IExceptionLogger logger)
        {
            _handler = handler;
            _logger = logger;
        }

        /* => PRETRAGA KATEGORIJA */
        // GET: api/<CategoriesController>    
        [HttpGet]
        public IActionResult Get([FromQuery] PageSearch search, 
                                 [FromServices] IGetCategoriesQuery query)
        {
             return Ok(_handler.HandleQuery(query, search));
        }

        /* => KREIRANJE KATEGORIJE */
        [HttpPost]
        public IActionResult Post(
            [FromBody] CreateCategoryDTO dto,
            [FromServices] ICreateCategoryCommand command)
        {
                _handler.HandleCommand(command, dto);
                return StatusCode(201);
        }
        // MODIFIKOVANJE KATEGORIJE
        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, 
                                [FromBody] CreateCategoryDTO dto,
                                [FromServices] IUpdateCategoryCommand command)
        {
            dto.Id = id;
            _handler.HandleCommand(command, dto);
            return StatusCode(200);
        }
        // BRISANJE KATEGORIJE
        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteCategoryCommand command)
        {
            _handler.HandleCommand(command, id);
            return NoContent();
        }
    }
}
