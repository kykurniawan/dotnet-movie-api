using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Models;

[Table("tags")]
public class Tag: Base
{
    [Column(name: "id")]
    public Guid Id { get; set; }

    [Column(name: "name")]
    public string Name { get; set; } = null!;

    public List<MovieTag> MovieTags { get; set; } = [];
}