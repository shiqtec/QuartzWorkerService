using Quartz;
using QuartzWorkerService;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration))
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
