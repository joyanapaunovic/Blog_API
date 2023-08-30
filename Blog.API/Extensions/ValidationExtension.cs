using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Blog.API.Extensions
{
    public static class ValidationExtension
    {
        public static UnprocessableEntityObjectResult AsUnprocessableEntity(this IEnumerable<ValidationFailure> errors)
        {
            var errorObject = new
            {
                errors = errors.Select(x => new
                {
                    property = x.PropertyName,
                    errorMessage = x.ErrorMessage
                })
            };

            var result = new UnprocessableEntityObjectResult(errorObject);

            return result;

        }
    }
}
