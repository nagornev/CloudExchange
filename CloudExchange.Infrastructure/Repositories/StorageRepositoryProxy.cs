using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CloudExchange.Infrastructure.Repositories
{
    public class StorageRepositoryProxy : IStorageRepository
    {
        private const string _internalServerMessage = "The storage repository is unavailable.";

        private readonly StorageRepository _storageRepository;

        private readonly ILogger<StorageRepositoryProxy> _logger;

        public StorageRepositoryProxy(StorageRepository storageRepository,
                                      ILogger<StorageRepositoryProxy> logger)
        {
            _storageRepository = storageRepository;
            _logger = logger;
        }

        public async Task<Result<Stream>> Get(Descriptor descriptor)
        {
            try
            {
                return await _storageRepository.Get(descriptor);
            }
            catch (Exception exception)
            {
                return HandleException<Stream>(exception);
            }
        }

        public async Task<Result> Create(Descriptor descriptor, Stream stream)
        {
            try
            {
                return await _storageRepository.Create(descriptor, stream);
            }
            catch (Exception exception)
            {
                return HandleException(exception);
            }
        }

        public async Task<Result> Delete(Descriptor decriptor)
        {
            try
            { 
                return await _storageRepository.Delete(decriptor);
            }
            catch(Exception exception)
            {
                return HandleException(exception);
            }
        }


        private Result HandleException(Exception exception)
        {
            LogError(exception);

            return Result.Failure(error => error.InternalServer(_internalServerMessage));
        }

        private Result<T> HandleException<T>(Exception exception)
        {
            LogError(exception);

            return Result<T>.Failure(error => error.InternalServer(_internalServerMessage));
        }

        private void LogError(Exception exception)
        {
            _logger.LogError(exception, _storageRepository.GetType().FullName);
        }
    }
}
