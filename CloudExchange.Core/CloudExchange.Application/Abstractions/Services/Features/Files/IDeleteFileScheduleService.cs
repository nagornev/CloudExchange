using CloudExchange.OperationResults;

namespace CloudExchange.Application.Abstractions.Services.Features.Files
{
    public interface IDeleteFileScheduleService
    {
        Task<Result> ScheduleDeleteFile(Guid descriptorId,
                                        TimeSpan delay,
                                        CancellationToken cancellation = default);
    }
}
