using System.Net;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Helpers;
using MovieApi.Http.Requests.Queue;
using MovieApi.Http.Responses;
using MovieApi.Infrastructure.Queue;

namespace MovieApi.Http.Controllers.V1;

[Route("api/v1/queue-example")]
[ApiController]
public class QueueController(IBackgroundTaskQueue backgroundTaskQueue, ILogger<QueueController> logger)
{
    protected readonly IBackgroundTaskQueue _backgroundTaskQueue = backgroundTaskQueue;
    protected readonly ILogger<QueueController> _logger = logger;

    [HttpPost()]
    public ApiResponse<QueueCreateRequest> Enqueue([FromBody] QueueCreateRequest queueCreateRequest)
    {
        // Enqueue a background task
        _backgroundTaskQueue.QueueBackgroundWorkItem(async (cancellationToken) =>
        {
            // Simulate a long running task
            _logger.LogInformation("Task Started: {Message} at {Time}", queueCreateRequest.Message, DateTime.Now);

            await Task.Delay(5000, cancellationToken);

            _logger.LogInformation("Task Completed: {Message} at {Time}", queueCreateRequest.Message, DateTime.Now);
        });
        
        return ApiResponseData.Create(HttpStatusCode.OK, queueCreateRequest, "Task enqueued successfully.");
    }
}
