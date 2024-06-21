using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Models;

[Table("movies")]
public class Movie: Base
{
    [Column(name: "id")]
    public Guid Id { get; set; }

    [Column(name: "title")]
    public string Title { get; set; } = null!;

    [Column(name:"overview", TypeName = "text")]
    public string Overview { get; set; } = null!;

    [Column(name: "poster")]
    public string Poster { get; set; } = null!;

    [Column(name:"play_until")]
    public DateTime PlayUntil { get; set; }

    public List<MovieTag> MovieTags { get; set; } = [];
}