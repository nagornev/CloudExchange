using CloudExchange.Backgrounds.Abstractions.Processors;

namespace CloudExchange.API.Backgrounds
{
    public class BackgroundProccessorsStarter : BackgroundService
    {
        private readonly IEnumerable<IBackgroundProcessor> _backgroundProcessors;

        public BackgroundProccessorsStarter(IEnumerable<IBackgroundProcessor> backgroundProcessors)
        {
            _backgroundProcessors = backgroundProcessors;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.WhenAll(_backgroundProcessors.Select(processor => processor.StartAsync(stoppingToken)));
        }
    }
}
