﻿using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Providers;
using CloudExchange.UseCases.Services;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Result<IEnumerable<Descriptor>> descriptorsResult = await _serverFileService.GetDescriptors(_timeProvider.NowUnix() + interval);

            if (descriptorsResult.Success)
                _ = descriptorsResult.Content.Select(ScheduleDelete);

            return Result.Successful();
        }

        public string ScheduleDelete(Descriptor descriptor)
        {
            return BackgroundJob.Schedule(() => Delete(descriptor),
                                                GetDelay(descriptor));
        }

        public async Task Delete(Descriptor descriptor)
        {
            _ = await _serverFileService.DeleteFile(descriptor.Id);
        }

        private TimeSpan GetDelay(Descriptor descriptor)
        {
            return TimeSpan.FromSeconds(_timeProvider.NowUnix() - descriptor.Uploaded < descriptor.Lifetime ?
                                            descriptor.Uploaded + descriptor.Lifetime - _timeProvider.NowUnix() :
                                            0.001);
        }
    }
}
