namespace MovieApi.Http.Middlewares;

public class ExampleMiddleware(RequestDelegate next, ILogger<ExampleMiddleware> logger)
{
    private readonly RequestDelegate _next = next;

    private readonly ILogger<ExampleMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        // Do something before
        _logger.LogInformation("ExampleMiddleware executing...");
        await _next(context);
        // Do something after
    }
}

public static class ExampleMiddlewareExtensions
{
    public static IApplicationBuilder UseExampleMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExampleMiddleware>();
    }
}