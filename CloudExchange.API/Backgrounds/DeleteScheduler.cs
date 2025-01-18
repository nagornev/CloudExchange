using CloudExchange.Domain.Models;
using CloudExchange.UseCases.Services;

namespace CloudExchange.API.Backgrounds
{
    public class DeleteScheduler : BackgroundService
    {
        private const int _interval = Descriptor.LifetimeMinumum;

        private readonly IServiceProvider _serviceProvider;

        public DeleteScheduler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using(var scope =  _serviceProvider.CreateScope())
                {
                    ISchedulerService schedulerService = scope.ServiceProvider.GetRequiredService<ISchedulerService>();

                    _ = await schedulerService.ScheduleDelete(_interval);

                    await Task.Delay(_interval * 1000);
                }
            }
        }
    }
}
