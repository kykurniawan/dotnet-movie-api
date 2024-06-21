using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieApi.Exceptions;
using MovieApi.Helpers;

namespace MovieApi.Http.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    protected readonly ILogger<ExceptionFilter> _logger = logger;

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An error occurred: {Message}", context.Exception.Message);

        if (context.Exception is DataNotFoundException dataNotFoundException)
        {
            context.Result = new ObjectResult(ApiResponseData.Create<object>(HttpStatusCode.NotFound, null, dataNotFoundException.Message))
            {
                StatusCode = (int) HttpStatusCode.NotFound,
            };

            return;
        }

        if(context.Exception is ValidationException validationException)
        {
            ModelStateDictionary modelState = validationException.ModelState;
            context.Result = new ObjectResult(ApiResponseData.Create<object>(HttpStatusCode.BadRequest, null, "Validation failed.", modelState))
            {
                StatusCode = (int) HttpStatusCode.BadRequest,
            };
            return;
        }
        
        context.Result = new ObjectResult(ApiResponseData.Create<object>(HttpStatusCode.InternalServerError, null, "An error occurred."))
        {
            StatusCode = (int) HttpStatusCode.InternalServerError,
        };
    }
}