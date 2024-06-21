using MovieApi.Infrastructure.Database;

namespace MovieApi.Domain.Movie.Repositories;

public class MovieTagRepository(MovieApiDbContext movieApiDbContext)
{
    private readonly MovieApiDbContext _dbContext = movieApiDbContext;

    public List<Models.MovieTag> CreateMany(List<Models.MovieTag> movieTags)
    {
        _dbContext.MovieTags.AddRange(movieTags);
        _dbContext.SaveChanges();

        return movieTags;
    }

    public void DeleteMany(List<Models.MovieTag> movieTags)
    {
        _dbContext.MovieTags.RemoveRange(movieTags);
        _dbContext.SaveChanges();
    }
}