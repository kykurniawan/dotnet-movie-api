using MovieApi.Infrastructure.External.Tmdb;
using Quartz;

namespace MovieApi.Infrastructure.Job;

class GetNowPlayingMoviesJob(
    ILogger<GetNowPlayingMoviesJob> logger,
    ITmdbService tmdbService
    ) : IJob
{
    private readonly ILogger<GetNowPlayingMoviesJob> _logger = logger;
    private readonly ITmdbService _tmdbService = tmdbService;

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("GetNowPlayingMoviesJob is running at {time}", DateTime.Now);

        var movies = await _tmdbService.GetNowPlayingMovies();

        _logger.LogInformation("Found {count} now playing movies", movies.Count);

        foreach (var movie in movies)
        {
            _logger.LogInformation("Now Playing Movie: {title}", movie.Title);
        }
    }
}