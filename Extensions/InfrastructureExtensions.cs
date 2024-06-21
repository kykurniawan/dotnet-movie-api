using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieApi.Domain.Movie.Repositories;
using MovieApi.Infrastructure.Database;
using MovieApi.Infrastructure.External.Tmdb;
using MovieApi.Infrastructure.Job;
using MovieApi.Infrastructure.Queue;
using NReco.Logging.File;
using Quartz;

namespace MovieApi.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Logging(services, configuration);
        Database(services, configuration);
        Cron(services, configuration);
        Queue(services);
        External(services);

        return services;
    }

    private static void Database(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MovieApiDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("Default"));
        });
    }

    private static void Logging(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(loggingBuilder =>
        {
            string errorLogPath = $"Logs/{DateTime.Now:yyyy-MM-dd}-error.log";

            loggingBuilder.AddFile(errorLogPath, fileLoggerOpts =>
            {
                fileLoggerOpts.Append = true;
                fileLoggerOpts.FilterLogEntry = (msg) =>
                {
                    return msg.LogLevel == LogLevel.Error;
                };
            });
        });
    }

    private static void Cron(IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(opt =>
        {
            opt.AddJob<ExampleJob>(JobKey.Create(nameof(ExampleJob)));
            opt.AddTrigger(trigger =>
            {
                trigger
                    .ForJob(nameof(ExampleJob))
                    .WithIdentity(nameof(ExampleJob))
                    .WithCronSchedule(configuration["CronJobs:Example"]!)
                    .StartNow();
            });

            opt.AddJob<GetNowPlayingMoviesJob>(JobKey.Create(nameof(GetNowPlayingMoviesJob)));
           
            opt.AddTrigger(trigger =>
            {
                trigger
                    .ForJob(nameof(GetNowPlayingMoviesJob))
                    .WithIdentity(nameof(GetNowPlayingMoviesJob))
                    .WithCronSchedule(configuration["CronJobs:GetNowPlayingMovies"]!)
                    .StartNow();
            });
        });

        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
    }

    private static void Queue(IServiceCollection services)
    {
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddHostedService<BackgroundTaskService>();
    }

    private static void External(IServiceCollection services)
    {
        services.AddScoped<ITmdbService, TmdbService>();
    }
}