using System.Net;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Domain.Movie.Services;
using MovieApi.Helpers;
using MovieApi.Http.Requests.Movie;
using MovieApi.Http.Responses;
using MovieApi.Infrastructure.Database;

namespace MovieApi.Http.Controllers.V1;

[Route("api/v1/movies")]
[ApiController]
public class MovieController(MovieService movieService)
{
    protected readonly MovieService _movieService = movieService;

    [HttpGet()]
    public ApiResponse<IPaginationResult<MovieResponse>> Index([FromQuery] MovieIndexRequest request)
    {
        var result = _movieService.FindAll(request);

        return ApiResponseData.Create(HttpStatusCode.OK ,MovieResponse.MapPagination(result), "Movies retrieved successfully.");
    }
    
    [HttpGet("{id}")]
    public ApiResponse<MovieResponse> Show([FromRoute] Guid id)
    {
        var movie = _movieService.Find(id);

        return ApiResponseData.Create(HttpStatusCode.OK, MovieResponse.MapSingle(movie), "Movie retrieved successfully.");
    }

    [HttpPost()]
    public ApiResponse<MovieResponse> Store([FromBody] MovieCreateRequest request)
    {
        var movie = _movieService.Create(request);

        return ApiResponseData.Create(HttpStatusCode.Created, MovieResponse.MapSingle(movie), "Movie created successfully.");
    }

    [HttpPut("{id}")]
    public ApiResponse<MovieResponse> Update([FromBody] MovieUpdateRequest request, [FromRoute] Guid id)
    {
        var movie = _movieService.Find(id);

        movie = _movieService.Update(request, movie.Id);

        return ApiResponseData.Create(HttpStatusCode.OK, MovieResponse.MapSingle(movie), "Movie updated successfully.");
    }



    [HttpDelete("{id}")]
    public ApiResponse<object> Destroy([FromRoute] Guid id)
    {
        _movieService.Delete(id);

        return ApiResponseData.Create<object>(HttpStatusCode.NoContent, null, "Movie deleted successfully.");
    }
}