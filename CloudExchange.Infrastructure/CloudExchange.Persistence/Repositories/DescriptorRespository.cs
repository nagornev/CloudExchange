using CloudExchange.Domain.Aggregates;
using CloudExchange.Persistence.Contexts;
using DDD.Repositories;
using DDD.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.Persistence.Repositories
{
    public class DescriptorRespository : IRepository<DescriptorAggregate>
    {
        private readonly DescriptorContext _context;

        public DescriptorRespository(DescriptorContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DescriptorAggregate aggregate, CancellationToken cancellation = default)
        {
            await _context.Descriptors.AddAsync(aggregate, cancellation);
        }

        public IAsyncEnumerable<DescriptorAggregate> AsyncStream(ISpecification<DescriptorAggregate> specification)
        {
            return _context.Descriptors.Where(specification.ToExpression())
                                       .AsAsyncEnumerable();
        }

        public Task DeleteAsync(DescriptorAggregate aggregate, CancellationToken cancellation = default)
        {
            _context.Descriptors.Remove(aggregate);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(ISpecification<DescriptorAggregate> specification, CancellationToken cancellation = default)
        {
            return await _context.Descriptors.AnyAsync(specification.ToExpression(), cancellation);
        }

        public async Task<IReadOnlyCollection<DescriptorAggregate>> FindAsync(ISpecification<DescriptorAggregate> specification, CancellationToken cancellation = default)
        {
            return await _context.Descriptors.Where(specification.ToExpression())
                                             .ToArrayAsync();
        }

        public async Task<DescriptorAggregate?> GetAsync(ISpecification<DescriptorAggregate> specification, CancellationToken cancellation = default)
        {
            return await _context.Descriptors.FirstOrDefaultAsync(specification.ToExpression(), cancellation);
        }

        public IEnumerable<DescriptorAggregate> Stream(ISpecification<DescriptorAggregate> specification)
        {
            return _context.Descriptors.Where(specification.ToExpression())
                                       .AsEnumerable();
        }
    }
}
