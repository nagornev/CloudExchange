using CloudExchange.OperationResults;
using CloudExchange.UseCases.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CloudExchange.Infrastructure.Services
{
    public class SchedulerServiceProxy : ISchedulerService
    {
        private const string _internalServerMessage = "The scheduler service is unavailable.";

        private readonly ISchedulerService _schedulerService;

        private readonly ILogger<SchedulerServiceProxy> _logger;

        public SchedulerServiceProxy(SchedulerService schedulerService,
                                     ILogger<SchedulerServiceProxy> logger)
        {
            _schedulerService = schedulerService;
            _logger = logger;
        }

        public async Task<Result> ScheduleDelete(int interval)
        {
            try
            {
                return await _schedulerService.ScheduleDelete(interval);
            }
            catch (Exception exception)
            {
                return HandleException(exception);
            }
        }

        private Result HandleException(Exception exception)
        {
            LogError(exception);

            return Result.Failure(error => error.InternalServer(_internalServerMessage));
        }

        private void LogError(Exception exception)
        {
            _logger.LogError(exception, _schedulerService.GetType().FullName);
        }
    }
}
