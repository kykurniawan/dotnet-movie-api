using System.Net.Http.Headers;
using System.Text.Json;

namespace MovieApi.Infrastructure.External.Tmdb;

public class TmdbService(ILogger<TmdbService> logger, HttpClient httpClient, IConfiguration configuration) : ITmdbService
{
    private readonly ILogger<TmdbService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;

    private readonly IConfiguration _configuration = configuration;

    public async Task<List<TmdbMovie>> GetNowPlayingMovies()
    {
        var tmdbSetting = _configuration.GetSection("ExternalServices:Tmdb").Get<TmdbSetting>()!;

        var url = $"{tmdbSetting.BaseUrl}/movie/now_playing";

        var authentication = new AuthenticationHeaderValue("Bearer", tmdbSetting.Token);

        _httpClient.DefaultRequestHeaders.Authorization = authentication;

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Status code {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();

        var serialized = JsonSerializer.Deserialize<TmdbResponse<TmdbMovie>>(content)!;

        return serialized.Results;
    }
}