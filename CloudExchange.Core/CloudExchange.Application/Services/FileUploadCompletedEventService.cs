using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Domain.Aggregates;
using DDD.Repositories;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class FileUploadCompletedEventService : IFileUploadCompletedEventService
    {
        private readonly IDescriptorQueryService _descriptorQueryService;

        private readonly IUnitOfWork _unitOfWork;

        public FileUploadCompletedEventService(IDescriptorQueryService descriptorQueryService,
                                               IUnitOfWork unitOfWork)
        {
            _descriptorQueryService = descriptorQueryService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(Guid descriptorId, CancellationToken cancellation = default)
        {
            Result<DescriptorAggregate> descriptorResult = await _descriptorQueryService.GetUnconfimedDescriptorByIdAsync(descriptorId, cancellation);

            if (descriptorResult.IsFailure)
                return descriptorResult;

            DescriptorAggregate descriptor = descriptorResult.Content;

            descriptor.Confirm();
            await _unitOfWork.SaveAsync(cancellation);

            return Result.Success();
        }
    }
}
