using CloudExchange.Domain.Abstractions.Delegates;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudExchange.EntitiyFrameworkCore.Repositories
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

        public async Task<Result<IEnumerable<DescriptorEntity>>> GetAsync(CancellationToken cancellation = default)
        {
            try
            {
                return await _descriptorRepository.GetAsync(cancellation);
            }
            catch (OperationCanceledException)
            {
                return Result<IEnumerable<DescriptorEntity>>.Failure(Errors.Interrupted("The operation to get all descriptors was interrupted."));
            }
            catch (Exception exception)
            {
                LogError(exception);
                return Result<IEnumerable<DescriptorEntity>>.Failure(Errors.ServiceUnavailable($"The descriptors database is unavailable."));
            }
        }

        public async Task<Result<DescriptorEntity>> GetAsync(Guid descriptorId, CancellationToken cancellation = default)
        {
            try
            {
                return await _descriptorRepository.GetAsync(descriptorId, cancellation);
            }
            catch (OperationCanceledException)
            {
                return Result<DescriptorEntity>.Failure(Errors.Interrupted($"The operation to get descriptor ({descriptorId}) was interrupted."));
            }
            catch (Exception exception)
            {
                LogError(exception);
                return Result<DescriptorEntity>.Failure(Errors.ServiceUnavailable($"The descriptors database is unavailable."));
            }
        }

        public async Task<Result<IAsyncEnumerable<DescriptorEntity>>> GetAsync(long deathTime, CancellationToken cancellation = default)
        {
            try
            {
                return await _descriptorRepository.GetAsync(deathTime, cancellation);
            }
            catch (OperationCanceledException)
            {
                return Result<IAsyncEnumerable<DescriptorEntity>>.Failure(Errors.Interrupted($"The operation to get dying descriptors ({deathTime}) was interrupted."));
            }
            catch (Exception exception)
            {
                LogError(exception);
                return Result<IAsyncEnumerable<DescriptorEntity>>.Failure(Errors.ServiceUnavailable($"The descriptors database is unavailable."));
            }
        }

        public async Task<Result> CreateAsync(DescriptorEntity descriptor, TransactionCreateAsyncDelegate callback, CancellationToken cancellation = default)
        {
            try
            {
                return await _descriptorRepository.CreateAsync(descriptor, callback, cancellation);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure(Errors.Interrupted($"The operation to create descriptor ({descriptor.Name}) was interrupted."));
            }
            catch (DbUpdateConcurrencyException exception)
            {
                LogError(exception);
                return Result.Failure(Errors.TransactionFailed($"The operation to create the descriptor failed."));
            }
            catch (DbUpdateException exception)
            {
                LogError(exception);
                return Result.Failure(Errors.TransactionFailed($"The operation to create the descriptor failed."));
            }
            catch (Exception exception)
            {
                LogError(exception);
                return Result.Failure(Errors.ServiceUnavailable($"The descriptors database is unavailable."));
            }
        }

        public async Task<Result> DeleteAsync(DescriptorEntity descriptor, TransactionDeleteAsyncDelegate callback, CancellationToken cancellation = default)
        {
            try
            {
                return await _descriptorRepository.DeleteAsync(descriptor, callback, cancellation);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure(Errors.Interrupted($"The operation to delete descriptor ({descriptor.Id}) was interrupted."));
            }
            catch (DbUpdateConcurrencyException exception)
            {
                LogError(exception);
                return Result.Failure(Errors.TransactionFailed($"The operation to delete the descriptor failed."));
            }
            catch (DbUpdateException exception)
            {
                LogError(exception);
                return Result.Failure(Errors.TransactionFailed($"The operation to delete the descriptor failed."));
            }
            catch (Exception exception)
            {
                LogError(exception);
                return Result.Failure(Errors.ServiceUnavailable($"The descriptors database is unavailable."));
            }
        }

        private void LogError(Exception exception, string message = "")
        {
            _logger.LogError(exception, string.IsNullOrEmpty(message) ? exception.Message : message);
        }
    }
}
