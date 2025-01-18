using CloudExchange.OperationResults;
using System.Threading.Tasks;

namespace CloudExchange.UseCases.Services
{
    public interface ISchedulerService
    {
        /// <summary>
        /// Schedules to delete files that will die at now+interval time.
        /// </summary>
        /// <param name="deathTime">Unix time in seconds.</param>
        /// <returns></returns>
        Task<Result> ScheduleDelete(int interval);
    }
}
