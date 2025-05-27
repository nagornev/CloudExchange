using CloudExchange.Application.Features.Files.Commands.ScheduleDeleteFile;
using CloudExchange.Domain.Entities;
using MediatR;

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
                    IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    _ = await mediator.Send(new ScheduleDeleteFileCommand(_interval), 
                                            stoppingToken);
                }
            }
        }
    }
}
