using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Providers;
using CloudExchange.UseCases.Services;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudExchange.Infrastructure.Services
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
            try
            {
                long deathTime = _timeProvider.NowUnix() + interval;

                Result<IEnumerable<Descriptor>> dyingResult = await _serverFileService.GetDescriptors(deathTime);

                if (dyingResult.Success)
                {
                    foreach (Descriptor descriptor in dyingResult.Content)
                    {
                        _ = CreateJob(descriptor);
                    }
                }

                return Result.Successful();
            }
            catch
            {
                return Result.Failure();
            }
        }

        public string CreateJob(Descriptor descriptor)
        {
            return BackgroundJob.Schedule(() => Delete(descriptor),
                                          TimeSpan.FromSeconds(_timeProvider.NowUnix() - descriptor.Uploaded < descriptor.Lifetime ?
                                                                  descriptor.Uploaded + descriptor.Lifetime - _timeProvider.NowUnix() :
                                                                  1));
        }

        public async Task Delete(Descriptor descriptor)
        {
            await _serverFileService.DeleteFile(descriptor.Id);
        }
    }
}
