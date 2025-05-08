using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Delegates;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using Microsoft.Extensions.Logging;

namespace CloudExchange.EntitiyFrameworkCorePersistence.Repositories
{
    public class DescriptorRepositoryExceptionDecorator : IDescriptorRepository
    {
        private readonly IDescriptorRepository _descriptorRepository;

        private readonly ILogger<DescriptorRepositoryExceptionDecorator> _logger;

        public DescriptorRepositoryExceptionDecorator(IDescriptorRepository descriptorRepository, 
                                                      ILogger<DescriptorRepositoryExceptionDecorator> logger)
        {
            _descriptorRepository = descriptorRepository;
            _logger = logger;
        }

        public Task<Result<IEnumerable<DescriptorEntity>>> GetAsync(CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<DescriptorEntity>> GetAsync(Guid descriptorId, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IAsyncEnumerable<DescriptorEntity>>> GetAsync(long deathTime, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CreateAsync(DescriptorEntity descriptor, TransactionCreateAsyncDelegate callback, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(DescriptorEntity descriptor, TransactionDeleteAsyncDelegate callback, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
