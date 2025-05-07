using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Domain.Entities;

namespace CloudExchange.API.Backgrounds
{
    public class DeleteScheduler : BackgroundService
    {
        private const int _interval = DescriptorEntity.LifetimeMinumum;

        private readonly IServiceProvider _serviceProvider;

        public DeleteScheduler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    ISchedulerService schedulerService = scope.ServiceProvider.GetRequiredService<ISchedulerService>();

                    _ = await schedulerService.ScheduleDelete(_interval);

                    await Task.Delay(_interval * 1000);
                }
            }
        }
    }
}
