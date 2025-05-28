using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Services.Features.Files;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Commands.ScheduleDeleteFile
{
    public class ScheduleDeleteFileHandler(IDescriptorRepository _descriptorRepository,
                                           IDeleteFileScheduleService _deleteFileScheduleService,
                                           ITimeProvider _timeProvider)
        : IRequestHandler<ScheduleDeleteFileCommand, Result>
    {
        public async Task<Result> Handle(ScheduleDeleteFileCommand request, CancellationToken cancellationToken)
        {
            Result<IAsyncEnumerable<DescriptorEntity>> descriptorEntitiesResult = await _descriptorRepository.GetAsync(_timeProvider.NowTimestamp() + request.Interval);

            if (descriptorEntitiesResult.IsSuccess)
                await foreach (DescriptorEntity descriptorEntity in descriptorEntitiesResult.Content)
                    _ = _deleteFileScheduleService.ScheduleDeleteFile(descriptorEntity.Id,
                                                                      GetDelay(descriptorEntity),
                                                                      cancellationToken);

            await Task.Delay(request.Interval * 1000, cancellationToken);

            return Result.Success();
        }

        private TimeSpan GetDelay(DescriptorEntity descriptorEntity)
        {
            return TimeSpan.FromSeconds(_timeProvider.NowTimestamp() - descriptorEntity.Uploaded < descriptorEntity.Lifetime ?
                                            descriptorEntity.Uploaded + descriptorEntity.Lifetime - _timeProvider.NowTimestamp() :
                                            0.001);
        }
    }
}
