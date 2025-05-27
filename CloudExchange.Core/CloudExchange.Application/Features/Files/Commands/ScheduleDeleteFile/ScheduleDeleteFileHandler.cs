using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Services.Features.Files;
using CloudExchange.Application.Dto;
using CloudExchange.Application.Features.Descriptors.Queries.GetExpiringDescriptors;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Commands.ScheduleDeleteFile
{
    public class ScheduleDeleteFileHandler(IMediator _mediator,
                                           IDeleteFileScheduleService _deleteFileScheduleService,
                                           ITimeProvider _timeProvider)
        : IRequestHandler<ScheduleDeleteFileCommand, Result>
    {
        public async Task<Result> Handle(ScheduleDeleteFileCommand request, CancellationToken cancellationToken)
        {
            Result<IAsyncEnumerable<DescriptorDto>> descriptorsResult = await _mediator.Send(new GetExpiringDescriptorsQuery(_timeProvider.NowUnix() + request.Interval));

            if (descriptorsResult.IsSuccess)
                await foreach (DescriptorDto descriptor in descriptorsResult.Content)
                    _ = _deleteFileScheduleService.ScheduleDeleteFile(descriptor.Id,
                                                                      GetDelay(descriptor),
                                                                      cancellationToken);

            await Task.Delay(request.Interval * 1000, cancellationToken);

            return Result.Success();
        }

        private TimeSpan GetDelay(DescriptorDto descriptor)
        {
            return TimeSpan.FromSeconds(_timeProvider.NowUnix() - descriptor.Uploaded < descriptor.Lifetime ?
                                            descriptor.Uploaded + descriptor.Lifetime - _timeProvider.NowUnix() :
                                            0.001);
        }
    }
}
