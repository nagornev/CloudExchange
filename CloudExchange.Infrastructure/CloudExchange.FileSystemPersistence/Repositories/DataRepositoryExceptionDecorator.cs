using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.FileSystemPersistence.Repositories
{
    public class DataRepositoryExceptionDecorator : IDataRepository
    {
        private readonly IDataRepository _dataRepository;

        private readonly ILogger<DataRepositoryExceptionDecorator> _logger;

        public DataRepositoryExceptionDecorator(IDataRepository dataRepository,
                                                ILogger<DataRepositoryExceptionDecorator> logger)
        {
            _dataRepository = dataRepository;
            _logger = logger;
        }

        public async Task<Result<Stream>> GetAsync(DescriptorEntity descriptor, CancellationToken cancellation = default)
        {
            try
            {
                return await _dataRepository.GetAsync(descriptor, cancellation);
            }
            catch(Exception exception)
            {
                LogError(exception);
                return Result<Stream>.Failure(Errors.ServiceUnavailable("The data repository is unavailable."));
            }
        }

        public async Task<Result> CreateAsync(DescriptorEntity descriptor, Stream stream, CancellationToken cancellation = default)
        {
            try
            {
                return await _dataRepository.CreateAsync(descriptor, stream, cancellation);
            }
            catch (Exception exception)
            {
                LogError(exception);
                return Result.Failure(Errors.ServiceUnavailable("The data repository is unavailable."));
            }
        }

        public async Task<Result> DeleteAsync(DescriptorEntity descriptor, CancellationToken cancellation = default)
        {
            try
            {
                return await _dataRepository.DeleteAsync(descriptor, cancellation);
            }
            catch (Exception exception)
            {
                LogError(exception);
                return Result.Failure(Errors.ServiceUnavailable("The data repository is unavailable."));
            }
        }

        private void LogError(Exception exception, string message = "")
        {
            _logger.LogError(exception, string.IsNullOrEmpty(message) ? exception.Message : message);
        }
    }
}
