using MovieApi.Http.Requests.Movie;
using MovieApi.Http.Requests;
using MovieApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MovieApi.Domain.Movie.Repositories;

public partial class MovieRepository(MovieApiDbContext dbContext)
{
    protected readonly MovieApiDbContext _dbContext = dbContext;

    public Models.Movie Create(Models.Movie movie)
    {
        _dbContext.Movies.Add(movie);
        _dbContext.SaveChanges();

        return movie;
    }

    public Models.Movie Update(Models.Movie movie)
    {
        _dbContext.Movies.Update(movie);
        _dbContext.SaveChanges();

        return movie;
    }

    public Models.Movie? Find(Guid id)
    {
        return _dbContext.Movies
            .Include(x => x.MovieTags)
            .ThenInclude(x => x.Tag)
            .First(x => x.Id == id);
    }

    public void Delete(Models.Movie movie)
    {
        _dbContext.Movies.Remove(movie);
        _dbContext.SaveChanges();
    }
}

public partial class MovieRepository
{
    public IPaginationResult<Models.Movie> Paginate(MovieIndexRequest request)
    {
        int skip = (request.Page - 1) * request.PerPage;
        var query = _dbContext.Movies.AsQueryable();

        query = query
            .Include(x => x.MovieTags)
            .ThenInclude(x => x.Tag);

        query = Filter(query, request);
        query = Search(query, request);
        query = Sort(query, request);

        var data = query
            .Skip(skip)
            .Take(request.PerPage)
            .ToList();

        return new PaginationResult<Models.Movie>
        {
            Items = data,
            Total = data.Count,
            Page = request.Page,
            PerPage = request.PerPage,
        };
    }

    protected IQueryable<Models.Movie> Sort(IQueryable<Models.Movie> query, MovieIndexRequest request)
    {
        if (string.IsNullOrEmpty(request.SortBy))
        {
            request.SortBy = "created_at";
        }

        Dictionary<string, Func<Models.Movie, object?>> sortFunctions = new()
        {
            { "title", x => x.Title },
            { "created_at", x => x.CreatedAt },
            { "updated_at", x => x.UpdatedAt },
        };

        if (!sortFunctions.TryGetValue(request.SortBy, out var sortFunction))
        {
            return query;
        }

        query = request.Order == SortOrderEnum.Asc
            ? query.OrderBy(sortFunction).AsQueryable()
            : query.OrderByDescending(sortFunction).AsQueryable();

        return query;
    }

    protected IQueryable<Models.Movie> Search(IQueryable<Models.Movie> query, MovieIndexRequest request)
    {
        if (string.IsNullOrEmpty(request.Search))
        {
            return query;
        }

        query = query.Where(x => x.Title.Contains(request.Search));

        return query;
    }

    protected IQueryable<Models.Movie> Filter(IQueryable<Models.Movie> query, MovieIndexRequest request)
    {
        query = Search(query, request);
        query = Sort(query, request);

        return query;
    }
}