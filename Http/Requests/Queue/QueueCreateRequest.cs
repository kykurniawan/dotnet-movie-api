using System.ComponentModel.DataAnnotations;

namespace MovieApi.Http.Requests.Queue;

public class QueueCreateRequest
{
    [Required]
    [MaxLength(128)]
    public string Message { get; set; } = null!;
}