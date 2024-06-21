using Microsoft.EntityFrameworkCore;
using MovieApi.Http.Requests;
using MovieApi.Http.Requests.Tag;
using MovieApi.Infrastructure.Database;

namespace MovieApi.Domain.Movie.Repositories;

public partial class TagRepository(MovieApiDbContext dbContext)
{
    protected readonly MovieApiDbContext _dbContext = dbContext;

    public Models.Tag Create(Models.Tag tag)
    {
        _dbContext.Tags.Add(tag);
        _dbContext.SaveChanges();

        return tag;
    }

    public Models.Tag Update(Models.Tag tag)
    {
        _dbContext.Tags.Update(tag);
        _dbContext.SaveChanges();

        return tag;
    }

    public Models.Tag? Find(Guid id)
    {
        return _dbContext.Tags
        .Include(x => x.MovieTags)
        .ThenInclude(x => x.Movie)
        .First(x => x.Id == id);
    }

    public List<Models.Tag> FindMany(List<Guid> ids)
    {
        return [.. _dbContext.Tags.Where(x => ids.Contains(x.Id))];
    }

    public void Delete(Models.Tag tag)
    {
        _dbContext.Tags.Remove(tag);
        _dbContext.SaveChanges();
    }
}

public partial class TagRepository
{
    public IPaginationResult<Models.Tag> Paginate(TagIndexRequest request)
    {
        int skip = (request.Page - 1) * request.PerPage;
        var query = _dbContext.Tags.AsQueryable();

        query = Search(query, request);
        query = Sort(query, request);

        var data = query
            .Skip(skip)
            .Take(request.PerPage)
            .ToList();

        return new PaginationResult<Models.Tag>
        {
            Items = data,
            Total = data.Count,
            Page = request.Page,
            PerPage = request.PerPage,
        };
    }

    private IQueryable<Models.Tag> Search(IQueryable<Models.Tag> query, TagIndexRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x => x.Name.Contains(request.Search));
        }

        return query;
    }

    private IQueryable<Models.Tag> Sort(IQueryable<Models.Tag> query, TagIndexRequest request)
    {
        if (string.IsNullOrEmpty(request.SortBy))
        {
            request.SortBy = "created_at";
        }

        Dictionary<string, Func<Models.Tag, object?>> sortFunctions = new()
        {
            { "name", x => x.Name },
            { "created_at", x => x.CreatedAt },
            { "updated_at", x => x.UpdatedAt },
        };

        if(!sortFunctions.TryGetValue(request.SortBy, out var sortFunction))
        {
            return query;
        }

        query = request.Order == SortOrderEnum.Asc
            ? query.OrderBy(sortFunction).AsQueryable()
            : query.OrderByDescending(sortFunction).AsQueryable();

        return query;
    }
}