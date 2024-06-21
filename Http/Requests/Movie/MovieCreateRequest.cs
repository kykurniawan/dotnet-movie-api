using System.ComponentModel.DataAnnotations;

namespace MovieApi.Http.Requests.Movie;

public class MovieCreateRequest
{
    [Required]
    [MaxLength(128)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(1024)]
    public string Overview { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Poster { get; set; } = null!;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime PlayUntil { get; set; }
    
    public List<Guid> Tags { get; set; } = [];
}