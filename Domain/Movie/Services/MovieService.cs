using MovieApi.Domain.Movie.Repositories;
using MovieApi.Exceptions;
using MovieApi.Http.Requests.Movie;
using MovieApi.Infrastructure.Database;

namespace MovieApi.Domain.Movie.Services;

public class MovieService(MovieRepository movieRepository, TagRepository tagRepository, MovieTagRepository movieTagRepository)
{
    protected readonly MovieRepository _movieRepository = movieRepository;
    protected readonly TagRepository _tagRepository = tagRepository;
    protected readonly MovieTagRepository _movieTagRepository = movieTagRepository;

    public Models.Movie Create(MovieCreateRequest movieCreateRequest)
    {
        var movie = new Models.Movie
        {
            Title = movieCreateRequest.Title,
            Overview = movieCreateRequest.Overview,
            Poster = movieCreateRequest.Poster,
            PlayUntil = movieCreateRequest.PlayUntil
        };

        movie = _movieRepository.Create(movie);

        if (movieCreateRequest.Tags.Count > 0)
        {
            var tags = _tagRepository.FindMany(movieCreateRequest.Tags);

            var movieTags = tags.Select(tag => new Models.MovieTag
            {
                MovieId = movie.Id,
                TagId = tag.Id
            }).ToList();

            _movieTagRepository.CreateMany(movieTags);
        }

        return movie;
    }

    public Models.Movie Update(MovieUpdateRequest movieUpdateRequest, Guid id)
    {
        var movie = Find(id);

        movie.Title = movieUpdateRequest.Title;
        movie.Overview = movieUpdateRequest.Overview;
        movie.Poster = movieUpdateRequest.Poster;
        movie.PlayUntil = movieUpdateRequest.PlayUntil;

        movie = _movieRepository.Update(movie);

        if (movieUpdateRequest.Tags.Count > 0)
        {
            var tags = _tagRepository.FindMany(movieUpdateRequest.Tags);

            var movieTags = tags.Select(tag => new Models.MovieTag
            {
                MovieId = movie.Id,
                TagId = tag.Id
            }).ToList();

            _movieTagRepository.DeleteMany(movie.MovieTags);
            _movieTagRepository.CreateMany(movieTags);
        }

        return movie;
    }

    public Models.Movie Find(Guid id)
    {
        Models.Movie movie = _movieRepository.Find(id) ?? throw new DataNotFoundException($"Movie with ID {id} not found.");

        return movie;
    }

    public IPaginationResult<Models.Movie> FindAll(MovieIndexRequest request)
    {
        return _movieRepository.Paginate(request);
    }

    public void Delete(Guid id)
    {
        var movie = Find(id);

        if (movie.MovieTags.Count > 0)
        {
            _movieTagRepository.DeleteMany(movie.MovieTags);
        }

        _movieRepository.Delete(movie);
    }
}