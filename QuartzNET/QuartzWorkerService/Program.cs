using Quartz;
using QuartzWorkerService;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices((hostBuilder, services) =>
    {
        services.AddQuartz(quartzConfig =>
        {
            quartzConfig.UseMicrosoftDependencyInjectionJobFactory();

            var jobKey = new JobKey("MyJob");

            quartzConfig.AddJob<MyJob>(options => options.WithIdentity(jobKey));
            quartzConfig.AddTrigger(options => options
                .ForJob(jobKey)
                .WithIdentity("MyJob-trigger")
                .WithCronSchedule(hostBuilder.Configuration["Quartz:MyJob"]));
        });

        services.AddQuartzHostedService(
            q => q.WaitForJobsToComplete = true);
    })
    .Build();

await host.RunAsync();
