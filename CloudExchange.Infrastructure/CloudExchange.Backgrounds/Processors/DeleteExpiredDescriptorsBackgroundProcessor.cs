using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Options;
using CloudExchange.Backgrounds.Abstractions.Processors;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CloudExchange.Backgrounds.Processors
{
    public class DeleteExpiredDescriptorsBackgroundProcessor : IDeleteExpiredDescriptorsBackgroundProcessor
    {
        private const string _job = "delete-expired-descriptors";

        private readonly IServiceProvider _serviceProvider;

        private readonly DescriptorOptions _descriptorOptions;

        public DeleteExpiredDescriptorsBackgroundProcessor(IServiceProvider serviceProvider,
                                                           IOptions<DescriptorOptions> descriptorOptions)
        {
            _serviceProvider = serviceProvider;
            _descriptorOptions = descriptorOptions.Value;
        }

        public Task StartAsync(CancellationToken cancellation)
        {
            RecurringJob.AddOrUpdate(_job,
                                     () => ExecuteAsync(cancellation),
                                     _descriptorOptions.DeletionInterval);

            return Task.CompletedTask;
        }

        [AutomaticRetry(Attempts = 10, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task ExecuteAsync(CancellationToken cancellation = default)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IDeleteExpiredDescriptorsBackgroundService deleteExpiredDescriptorsBackgroundService = scope.ServiceProvider.GetRequiredService<IDeleteExpiredDescriptorsBackgroundService>();
                await deleteExpiredDescriptorsBackgroundService.DeleteAsync(cancellation);
            }
        }
    }
}
