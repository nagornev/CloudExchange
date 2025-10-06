using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Backgrounds.Abstractions.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace CloudExchange.Backgrounds.Processors
{
    public class OutboxBackgroundProcessor : IOutboxBackgroundProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public OutboxBackgroundProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    IOutboxService outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();

                    try
                    {
                        await outboxService.HandleAsync(cancellation);
                    }
                    catch
                    {
                        continue;
                    }

                }
            }
        }
    }
}
