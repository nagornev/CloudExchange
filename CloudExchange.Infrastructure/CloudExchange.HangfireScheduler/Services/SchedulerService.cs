using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using Hangfire;

namespace CloudExchange.HangfireScheduler.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IServerFileService _serverFileService;

        private readonly ITimeProvider _timeProvider;

        public SchedulerService(IServerFileService serverFileService,
                                ITimeProvider timeProvider)
        {
            _serverFileService = serverFileService;
            _timeProvider = timeProvider;
        }

        public async Task<Result> ScheduleDelete(int interval)
        {
            Result<IAsyncEnumerable<DescriptorEntity>> descriptorsResult = await _serverFileService.GetDescriptorsAsync(_timeProvider.NowUnix() + interval);

            if (descriptorsResult.IsSuccess)
                await foreach (DescriptorEntity descriptor in descriptorsResult.Content)
                    _ = ScheduleDelete(descriptor);

            return Result.Success();
        }

        public string ScheduleDelete(DescriptorEntity descriptor)
        {
            return BackgroundJob.Schedule(() => Delete(descriptor),
                                                GetDelay(descriptor));
        }

        public async Task Delete(DescriptorEntity descriptor)
        {
            _ = await _serverFileService.DeleteFileAsync(descriptor.Id);
        }

        private TimeSpan GetDelay(DescriptorEntity descriptor)
        {
            return TimeSpan.FromSeconds(_timeProvider.NowUnix() - descriptor.Uploaded < descriptor.Lifetime ?
                                            descriptor.Uploaded + descriptor.Lifetime - _timeProvider.NowUnix() :
                                            0.001);
        }
    }
}
