using CloudExchange.UseCases.Services;

namespace CloudExchange.API.Backgrounds
{
    public class DeleteScheduler : BackgroundService
    {
        public const int Interval = 10;

        private readonly ISchedulerService _schedulerService;

        public DeleteScheduler(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _ = await _schedulerService.ScheduleDelete(Interval);

                await Task.Delay(Interval * 1000);
            }
        }
    }
}
