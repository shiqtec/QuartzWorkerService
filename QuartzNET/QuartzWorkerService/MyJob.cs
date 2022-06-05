using Quartz;

namespace QuartzWorkerService
{
    [DisallowConcurrentExecution]
    public class MyJob : IJob
    {
        private readonly ILogger<MyJob> _logger;
        public MyJob(ILogger<MyJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting MyJob");
            return Task.CompletedTask;
        }
    }
}