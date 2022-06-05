# QuartzWorkerService

This is a simple example of using `Quartz.NET`.

`Serilog` is also used here, where all log configurations are read from appsettings.

Quartz.NET has three main concepts:
- **Job**: This is the background tasks that you want to run.
- **Trigger**: A trigger controls when a job runs, typically firing on some sort of schedule.
- **Scheduler**: This is responsible for coordinating the jobs and triggers, executing the jobs as required by the triggers.

## The Job

> In `Program.cs`:
> ```cs
> quartzConfig.UseMicrosoftDependencyInjectionJobFactory();
> 
> var jobKey = new JobKey("MyJob");
> 
> quartzConfig.AddJob<MyJob>(options => 
>    options.WithIdentity(jobKey));
>```

`UseMicrosoftDependencyInjectionJobFactory` registers an `IJobFactory` that creates jobs by fetching them from the DI container. These jobs will always be created as a scoped service.

We then add a job, *MyJob*, and give it an identity.

## The Trigger

> ```cs
> quartzConfig.AddTrigger(options => options
>             .ForJob(jobKey)
>             .WithIdentity("MyJob-trigger")
>             .WithCronSchedule(hostBuilder.Configuration["Quartz:MyJob"]));
>```

We add a trigger for our job and also give this trigger an identity. The trigger has a schedule defined in appsettings and follows the cron syntax.

## Hosted Service

> ```cs
> quartzConfig.AddTrigger(options => options
>             .ForJob(jobKey)
>             .WithIdentity("MyJob-trigger")
>             .WithCronSchedule(hostBuilder.Configuration["Quartz:MyJob"]));
>```

Finally we register Quartz as a hosted service where `WaitForJobsToComplete` is a setting that waits for Jobs to end gracefully before shutting down.
