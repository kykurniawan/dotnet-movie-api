namespace MovieApi.Infrastructure.External.Tmdb;

public interface ITmdbService
{
    Task<List<TmdbMovie>> GetNowPlayingMovies();
}