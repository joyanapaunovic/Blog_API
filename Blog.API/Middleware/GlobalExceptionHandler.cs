using Blog.Application.Exceptions;
using Blog.Application.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionLogger _logger;

        public GlobalExceptionHandler(RequestDelegate next, IExceptionLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var guid = Guid.NewGuid();
                object body = new { message = "There was an error processing your request. Please contact support with this identifier: " + guid };
                //object body = null;
                var statusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                // FORBIDDEN
                if (ex is ForbiddenUseCaseExecutionException fodbiddenEx)
                {
                    statusCode = StatusCodes.Status403Forbidden;
                    context.Response.StatusCode = statusCode;
                    body = new { message = fodbiddenEx.Message };
                }
                if (ex is InformationExceptionForUser infoEx)
                {
                    statusCode = StatusCodes.Status422UnprocessableEntity;
                    context.Response.StatusCode = statusCode;
                    body = new { message = infoEx.Message };
                }
                // ENTITY NOT FOUND
                if (ex is EntityNotFoundException entityNotFoundEx)
                {
                    statusCode = StatusCodes.Status404NotFound;
                    context.Response.StatusCode = statusCode;
                    body = new { message = entityNotFoundEx.Message };
                }
                // 422
                if (ex is ValidationException e)
                {
                    statusCode = StatusCodes.Status422UnprocessableEntity;
                    context.Response.StatusCode = statusCode;
                    body = new
                    {
                        errors = e.Errors.Select(x => new
                        {
                            property = x.PropertyName,
                            error = x.ErrorMessage
                        })
                    };
                }
                // USE CASE CONFLICT 
                if (ex is UseCaseConflictException conflictEx)
                {
                    statusCode = StatusCodes.Status409Conflict;
                    context.Response.StatusCode = statusCode;
                    body = new { message = conflictEx.Message };
                }

                _logger.Log(ex);

                if (body != null)
                {
                    await context.Response.WriteAsJsonAsync(body);
                }
            }
        }
    }
}
