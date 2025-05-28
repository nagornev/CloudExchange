using CloudExchange.Application.Abstractions.Services.Features.Files;
using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;
using Hangfire;

namespace CloudExchange.Hangfire.Services
{
    public class DeleteFileScheduleService : IDeleteFileScheduleService
    {
        private readonly IDeleteFileByServerService _deleteFileByServerService;

        public DeleteFileScheduleService(IDeleteFileByServerService deleteFileByServerService)
        {
            _deleteFileByServerService = deleteFileByServerService;
        }

        public Task<Result> ScheduleDeleteFile(Guid descriptorId,
                                               TimeSpan delay,
                                               CancellationToken cancellation = default)
        {
            string id = BackgroundJob.Schedule(() => Delete(descriptorId, cancellation),
                                               delay);

            return !string.IsNullOrEmpty(id) ?
                    Task.FromResult(Result.Success()) :
                    Task.FromResult(Result.Failure(Errors.NullOrEmpty($"Scheduler delete service can not create background job for {descriptorId} file.")));
        }

        public async Task Delete(Guid descriptorId,
                                 CancellationToken cancellation = default)
        {
            _ = await _deleteFileByServerService.DeleteFile(descriptorId, cancellation);
        }
    }
}
