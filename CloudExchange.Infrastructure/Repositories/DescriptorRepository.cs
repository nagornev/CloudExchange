using CloudExchange.Database.Contexts;
using CloudExchange.Database.Entities;
using CloudExchange.Database.Extensions;
using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Delegates;
using CloudExchange.UseCases.Providers;
using CloudExchange.UseCases.Repositories;
using Google.Protobuf.Reflection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudExchange.Infrastructure.Repositories
{
    public class DescriptorRepository : IDescriptorRepository
    {
        private const string _internalServerMessage = "The descriptor database is unavailable.";

        private readonly DescriptorContext _context;

        public DescriptorRepository(DescriptorContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Descriptor>>> Get()
        {
            DescriptorEntity[] descriptorEntities = await _context.Descriptors.AsNoTracking()
                                                                              .ToArrayAsync();

            return descriptorEntities.GetDomain();
        }

        public async Task<Result<Descriptor>> Get(Guid descriptorId)
        {
            DescriptorEntity? descriptorEntity = await _context.Descriptors.AsNoTracking()
                                                                               .FirstOrDefaultAsync(x => x.Id == descriptorId);

            return descriptorEntity is not null ?
                        descriptorEntity.GetDomain() :
                        Result<Descriptor>.Failure(error => error.NullOrEmpty($"The file {descriptorId} was not found."));
        }

        public async Task<Result<IEnumerable<Descriptor>>> Get(long deathTime)
        {
            DescriptorEntity[] descriptorEntities = await _context.Descriptors.AsNoTracking()
                                                                              .Where(x => (x.Uploaded + x.Lifetime) < deathTime)
                                                                              .ToArrayAsync();

            return descriptorEntities.GetDomain();
        }

        public async Task<Result> Create(Descriptor descriptor, TransactionCreateDelegate callback)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Descriptors.AddAsync(descriptor.GetEntity());

                    ResultCommand createCommand = ResultCommandBuilder.Create()
                                                                      .AddCommand(async () =>
                                                                      {
                                                                          return await _context.SaveChangesAsync() > 0 ?
                                                                                    Result.Successful() :
                                                                                    Result.Failure(error => error.InternalServer(_internalServerMessage));
                                                                      })
                                                                      .AddCommand(async () =>
                                                                      {
                                                                          return await callback.Invoke(descriptor);
                                                                      })
                                                                      .SetSuccessfulCallback(async () =>
                                                                      {
                                                                          await transaction.CommitAsync();
                                                                      })
                                                                      .SetFailureCallback(async () =>
                                                                      {
                                                                          await transaction.RollbackAsync();
                                                                      })
                                                                      .Build();

                    return await createCommand.Invoke();
                }
                catch
                {
                    await transaction.RollbackAsync();

                    return Result.Failure(error => error.Operation("The create operation was not executed."));
                }
            }
        }

        public async Task<Result> Delete(Descriptor descriptor, TransactionDeleteDelegate callback)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    ResultCommand deleteCommand = ResultCommandBuilder.Create()
                                                                  .AddCommand(async () =>
                                                                  {
                                                                      return await _context.Descriptors.Where(x => x.Id == descriptor.Id)
                                                                                                       .ExecuteDeleteAsync() > 0 ?
                                                                             Result.Successful() :
                                                                             Result.Failure(error => error.InternalServer(_internalServerMessage));
                                                                  })
                                                                  .AddCommand(async () =>
                                                                  {
                                                                      return await callback.Invoke(descriptor);
                                                                  })
                                                                  .SetSuccessfulCallback(async () => await transaction.CommitAsync())
                                                                  .SetFailureCallback(async () => await transaction.RollbackAsync())
                                                                  .Build();

                    return await deleteCommand.Invoke();
                }
                catch
                {
                    await transaction.RollbackAsync();

                    return Result.Failure(error => error.Operation("The delete operation was not executed."));
                }
            }
        }
    }
}
