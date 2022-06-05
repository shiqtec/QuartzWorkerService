using Quartz;
using QuartzWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilder, services) =>
    {
        services.AddQuartz(quartzConfig =>
        {
            quartzConfig.UseMicrosoftDependencyInjectionJobFactory();

            var jobKey = new JobKey("HelloWorldJob");

            quartzConfig.AddJob<MyJob>(options => options.WithIdentity(jobKey));
            quartzConfig.AddTrigger(options => options
                .ForJob(jobKey)
                .WithIdentity("HelloWorldJob-trigger")
                .WithCronSchedule(hostBuilder.Configuration["Quartz:HelloWorldJob"]));
        });

        services.AddQuartzHostedService(
            q => q.WaitForJobsToComplete = true);
    })
    .Build();

await host.RunAsync();
