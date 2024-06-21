using System.Text.Json;
using MovieApi.Http.Responses.Tag;
using MovieApi.Infrastructure.Database;
using MovieApi.Models;

namespace MovieApi.Http.Responses;

public class MovieResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Overview { get; set; } = null!;
    public string Poster { get; set; } = null!;
    public DateTime PlayUntil { get; set; }
    public List<TagResponse> Tags { get; set; } = [];
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static MovieResponse MapSingle(Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            Overview = movie.Overview,
            Poster = movie.Poster,
            PlayUntil = movie.PlayUntil,
            Tags = movie.MovieTags.Select(x => TagResponse.MapSingle(x.Tag)).ToList(),
            CreatedAt = movie.CreatedAt,
            UpdatedAt = movie.UpdatedAt,
        };
    }

    public static List<MovieResponse> MapMany(List<Movie> movies)
    {
        return movies.Select(MapSingle).ToList();
    }

    public static IPaginationResult<MovieResponse> MapPagination(IPaginationResult<Movie> pagination)
    {
        return new PaginationResult<MovieResponse>
        {
            Items = MapMany(pagination.Items),
            Total = pagination.Total,
            PerPage = pagination.PerPage,
            Page = pagination.Page,
        };
    }
}