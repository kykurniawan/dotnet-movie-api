using System.Text.Json.Serialization;

namespace MovieApi.Infrastructure.External.Tmdb;

public class TmdbResponse<T>
{
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("results")]
    public List<T> Results { get; set; } = null!;
    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }
}

public class TmdbMovie
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    [JsonPropertyName("overview")]
    public string Overview { get; set; } = null!;
    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; } = null!;
    [JsonPropertyName("backdrop_path")]
    public string BackdropPath { get; set; } = null!;
    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; } = null!;
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
    [JsonPropertyName("genre_ids")]
    public List<int> GenreIds { get; set; } = null!;
}