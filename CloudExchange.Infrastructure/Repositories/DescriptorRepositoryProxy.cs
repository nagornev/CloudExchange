using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Delegates;
using CloudExchange.UseCases.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudExchange.Infrastructure.Repositories
{
    public class DescriptorRepositoryProxy : IDescriptorRepository
    {
        private const string _internalServerMessage = "The descriptor repository is unavailable.";

        private readonly DescriptorRepository _descriptorRepository;

        private readonly ILogger<DescriptorRepositoryProxy> _logger;

        public DescriptorRepositoryProxy(DescriptorRepository descriptorRepository,
                                         ILogger<DescriptorRepositoryProxy> logger)
        {
            _descriptorRepository = descriptorRepository;
            _logger = logger;
        }
        public async Task<Result<IEnumerable<Descriptor>>> Get()
        {
            try
            {
                return await _descriptorRepository.Get();
            }
            catch (Exception exception)
            {
                return HandleException<IEnumerable<Descriptor>>(exception);
            }
        }

        public async Task<Result<Descriptor>> Get(Guid descriptorId)
        {
            try
            {
                return await _descriptorRepository.Get(descriptorId);
            }
            catch (Exception exception)
            {
                return HandleException<Descriptor>(exception);
            }
        }

        public async Task<Result<IEnumerable<Descriptor>>> Get(long deathTime)
        {
            try
            {
                return await _descriptorRepository.Get(deathTime);
            }
            catch (Exception exception)
            {
                return HandleException<IEnumerable<Descriptor>>(exception);
            }
        }


        public async Task<Result> Create(Descriptor descriptor, TransactionCreateDelegate callback)
        {
            try
            {
                return await _descriptorRepository.Create(descriptor, callback);
            }
            catch (Exception exception)
            {
                return HandleException(exception);
            }
        }

        public async Task<Result> Delete(Descriptor descriptor, TransactionDeleteDelegate callback)
        {
            try
            {
                return await _descriptorRepository.Delete(descriptor, callback);
            }
            catch (Exception exception)
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
            _logger.LogError(exception, _descriptorRepository.GetType().FullName);
        }
    }
}
