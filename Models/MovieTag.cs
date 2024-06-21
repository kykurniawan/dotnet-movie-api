using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Models;

[Table("movie_tags")]
public class MovieTag : Base
{
    [Column(name: "id")]
    public Guid Id { get; set; }

    [Column(name: "movie_id")]
    public Guid MovieId { get; set; }

    [Column(name: "tag_id")]
    public Guid TagId { get; set; }

    public Movie Movie { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}