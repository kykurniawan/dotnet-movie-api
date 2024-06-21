using MovieApi.Infrastructure.Database;
using MovieApi.Models;

namespace MovieApi.Http.Responses.Tag;

public class TagResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static TagResponse MapSingle(Models.Tag tag)
    {
        if (tag == null)
        {
            return new TagResponse();
        }
        
        return new TagResponse
        {
            Id = tag.Id,
            Name = tag.Name,
            CreatedAt = tag.CreatedAt,
            UpdatedAt = tag.UpdatedAt,
        };
    }

    public static List<TagResponse> MapMany(List<Models.Tag> tags)
    {
        return tags.Select(MapSingle).ToList();
    }

    public static IPaginationResult<TagResponse> MapPagination(IPaginationResult<Models.Tag> tags)
    {
        return new PaginationResult<TagResponse>
        {
            Items = MapMany(tags.Items),
            Total = tags.Total,
            Page = tags.Page,
            PerPage = tags.PerPage,
        };
    }
}