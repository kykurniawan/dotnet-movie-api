using Quartz;

namespace MovieApi.Infrastructure.Job;

class ExampleJob(ILogger<ExampleJob> logger): IJob
{
    private readonly ILogger<ExampleJob> _logger = logger;

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("ExampleJob is running at {time}", DateTime.Now);
        
        await Task.CompletedTask;
    } 
}