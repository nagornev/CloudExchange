using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Commands.ScheduleDeleteFile
{
    public record ScheduleDeleteFileCommand(int Interval)
        : IRequest<Result>;
}
