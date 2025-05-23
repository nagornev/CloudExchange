﻿using CloudExchange.OperationResults;

namespace CloudExchange.Application.Abstractions.Services
{
    public interface ISchedulerService
    {
        /// <summary>
        /// Schedules to delete files that will die at now+interval time.
        /// </summary>
        /// <param name="interval">Unix time in seconds.</param>
        /// <returns></returns>
        Task<Result> ScheduleDelete(int interval);
    }
}
