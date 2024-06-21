using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MovieApi.Exceptions;

public class ValidationException(string message, ModelStateDictionary modelState) : Exception(message)
{
    public ModelStateDictionary ModelState { get; set; } = modelState;
}