using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Providers;
using CloudExchange.UseCases.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudExchange.Infrastructure.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ITimeProvider _timeProvider;

        public SchedulerService(IServiceProvider serviceProvider,
                                ITimeProvider timeProvider)
        {
            _serviceProvider = serviceProvider;
            _timeProvider = timeProvider;
        }

        public async Task<Result> ScheduleDelete(int interval)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IServerFileService serverFileService = scope.ServiceProvider.GetRequiredService<IServerFileService>();

                try
                {
                    long deathTime = _timeProvider.NowUnix() + interval;

                    Result<IEnumerable<Descriptor>> dyingResult = await serverFileService.GetDescriptors(deathTime);

                    if (dyingResult.Success)
                    {
                        foreach (Descriptor descriptor in dyingResult.Content)
                        {
                            var id = CreateJob(descriptor);
                        }
                    }

                    return Result.Successful();
                }
                catch
                {
                    return Result.Failure();
                }
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
            await Task.Run(async () =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    IServerFileService serverFileService = scope.ServiceProvider.GetRequiredService<IServerFileService>();

                    _ = await serverFileService.DeleteFile(descriptor.Id);
                }
            });
        }
    }
}
