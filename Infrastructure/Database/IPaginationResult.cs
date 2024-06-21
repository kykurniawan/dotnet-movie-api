namespace MovieApi.Infrastructure.Database;

public interface IPaginationResult<T>
{
    public List<T> Items { get; set; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
}