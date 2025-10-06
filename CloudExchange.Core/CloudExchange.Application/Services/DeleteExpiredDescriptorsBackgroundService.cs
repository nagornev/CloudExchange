using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Domain.Aggregates;
using DDD.Repositories;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class DeleteExpiredDescriptorsBackgroundService : IDeleteExpiredDescriptorsBackgroundService
    {
        private readonly IDescriptorQueryService _descriptorQueryService;

        private readonly ITimeProvider _timeProvider;

        private readonly IUnitOfWork _unitOfWork;

        public DeleteExpiredDescriptorsBackgroundService(IDescriptorQueryService descriptorQueryService,
                                                         ITimeProvider timeProvider,
                                                         IUnitOfWork unitOfWork)
        {
            _descriptorQueryService = descriptorQueryService;
            _timeProvider = timeProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteAsync(CancellationToken cancellation = default)
        {
            Result<IReadOnlyCollection<DescriptorAggregate>> expiredDecriptorsResult = await _descriptorQueryService.GetExpiredDescriptorsAsync(_timeProvider.NowUnix(), cancellation);

            if (expiredDecriptorsResult.IsFailure)
                throw new InvalidOperationException(expiredDecriptorsResult.Error.Message);

            foreach (DescriptorAggregate expiredDescriptor in expiredDecriptorsResult.Content)
            {
                expiredDescriptor.MarkAsDeleted();
            }

            await _unitOfWork.SaveAsync(cancellation);
        }
    }
}
