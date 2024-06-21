using MovieApi.Domain.Movie.Repositories;

namespace MovieApi.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<MovieRepository>();
        services.AddScoped<TagRepository>();
        services.AddScoped<MovieTagRepository>();

        return services;
    }
}