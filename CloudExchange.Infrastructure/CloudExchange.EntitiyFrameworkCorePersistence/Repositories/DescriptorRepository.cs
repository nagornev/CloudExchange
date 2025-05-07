using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Delegates;
using CloudExchange.Domain.Entities;
using CloudExchange.EntitiyFrameworkCorePersistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.EntitiyFrameworkCorePersistence.Repositories
{
    public class DescriptorRepository : IDescriptorRepository
    {
        private readonly DescriptorContext _context;

        public DescriptorRepository(DescriptorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DescriptorEntity>> Get()
        {
            return await _context.Descriptors.ToArrayAsync();
        }

        public async Task<DescriptorEntity?> Get(Guid descriptorId)
        {
            return await _context.Descriptors.FirstOrDefaultAsync(x => x.Id == descriptorId);
        }

        public Task<IAsyncEnumerable<DescriptorEntity>> Get(long deathTime)
        {
            return Task.FromResult(_context.Descriptors.Where(x => (x.Uploaded + x.Lifetime) < deathTime)
                                                       .AsAsyncEnumerable());
        }

        public async Task<bool> Create(DescriptorEntity descriptor, TransactionCreateDelegate callback)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _ = await _context.Descriptors.AddAsync(descriptor);

                    if (!(await _context.SaveChangesAsync() > 0) ||
                        !(await callback.Invoke(descriptor)))
                        await transaction.RollbackAsync();

                    await transaction.CommitAsync();

                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                }
            }

            return false;
        }

        public async Task<bool> Delete(DescriptorEntity descriptor, TransactionDeleteDelegate callback)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!(await _context.Descriptors.Where(x=>x.Id==descriptor.Id)
                                                    .ExecuteDeleteAsync() > 0) ||
                        !(await callback.Invoke(descriptor)))
                        await transaction.RollbackAsync();

                    await transaction.CommitAsync();

                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                }

                return false;
            }
        }
    }
}
