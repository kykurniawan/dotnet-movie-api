using MovieApi.Domain.Movie.Services;

namespace MovieApi.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<MovieService>();
        services.AddScoped<TagService>();

        return services;
    }
}