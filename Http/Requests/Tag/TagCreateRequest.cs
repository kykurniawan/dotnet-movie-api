using System.ComponentModel.DataAnnotations;

namespace MovieApi.Http.Requests.Tag;

public class TagCreateRequest
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = null!;
}