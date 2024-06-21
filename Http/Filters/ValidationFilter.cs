using Microsoft.AspNetCore.Mvc.Filters;
using MovieApi.Exceptions;

namespace MovieApi.Http.Filters;

public class ValidationFilter: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            throw new ValidationException("Validation Error", context.ModelState);
        }

        base.OnActionExecuting(context);
    }
}