﻿using CloudExchange.Domain.Abstractions.Delegates;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.Domain.Failures;
using CloudExchange.EntitiyFrameworkCore.Contexts;
using CloudExchange.OperationResults;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.EntitiyFrameworkCore.Repositories
{
    public class DescriptorRepository : IDescriptorRepository
    {
        private readonly DescriptorContext _context;

        public DescriptorRepository(DescriptorContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<DescriptorEntity>>> GetAsync(CancellationToken cancellation = default)
        {
            DescriptorEntity[] descriptors = await _context.Descriptors.ToArrayAsync(cancellation);

            return Result<IEnumerable<DescriptorEntity>>.Success(descriptors);
        }

        public async Task<Result<DescriptorEntity>> GetAsync(Guid descriptorId,
                                                             CancellationToken cancellation = default)
        {
            DescriptorEntity? descriptor = await _context.Descriptors.FirstOrDefaultAsync(x => x.Id == descriptorId,
                                                                                          cancellation);


            return descriptor != null ?
                      Result<DescriptorEntity>.Success(descriptor) :
                      Result<DescriptorEntity>.Failure(Errors.NotFound($"The descriptor {descriptorId} was not found."));
        }

        public Task<Result<IAsyncEnumerable<DescriptorEntity>>> GetAsync(long expiringTime,
                                                                         CancellationToken cancellation = default)
        {
            IAsyncEnumerable<DescriptorEntity> descriptors = _context.Descriptors.Where(x => (x.Uploaded + x.Lifetime) < expiringTime)
                                                                                 .AsAsyncEnumerable();

            return Task.FromResult(Result<IAsyncEnumerable<DescriptorEntity>>.Success(descriptors));
        }

        public async Task<Result> CreateAsync(DescriptorEntity descriptor,
                                              TransactionCreateAsyncDelegate callback,
                                              CancellationToken cancellation = default)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellation))
            {
                _ = await _context.Descriptors.AddAsync(descriptor, cancellation);

                try
                {
                    Result createResult = await _context.SaveChangesAsync(cancellation) > 0 ?
                                             await callback.Invoke(descriptor, cancellation) :
                                             Result.Failure(Errors.TransactionFailed("The creation transaction failed."));

                    if (createResult.IsFailure)
                    {
                        await transaction.RollbackAsync(cancellation);
                        return createResult;
                    }

                    await transaction.CommitAsync(cancellation);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellation);
                    throw;
                }
            }

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(DescriptorEntity descriptor,
                                              TransactionDeleteAsyncDelegate callback,
                                              CancellationToken cancellation = default)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellation))
            {
                try
                {
                    Result deleteResult = await _context.Descriptors.Where(x => x.Id == descriptor.Id)
                                                                    .ExecuteDeleteAsync(cancellation) > 0 ?
                                                    await callback.Invoke(descriptor, cancellation) :
                                                    Result.Failure(Errors.TransactionFailed("The deletion transaction failed."));

                    if (deleteResult.IsFailure)
                    {
                        await transaction.RollbackAsync(cancellation);
                        return deleteResult;
                    }

                    await transaction.CommitAsync(cancellation);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellation);
                    throw;
                }
            }

            return Result.Success();
        }
    }
}
