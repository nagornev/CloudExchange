using CloudExchange.Domain.Aggregates;
using CloudExchange.Persistence.Configurations;
using CloudExchange.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.Persistence.Contexts
{
    public class DescriptorContext : DbContext
    {
        public DescriptorContext(DbContextOptions<DescriptorContext> options) :
            base(options)
        {
        }

        public DbSet<DescriptorAggregate> Descriptors { get; set; }

        public DbSet<OutboxMessage> Outbox { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DescriptorEntityConfiguration());
        }
    }
}
