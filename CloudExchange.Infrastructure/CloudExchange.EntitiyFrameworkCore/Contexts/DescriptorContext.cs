using CloudExchange.Domain.Entities;
using CloudExchange.EntitiyFrameworkCore.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.EntitiyFrameworkCore.Contexts
{
    public class DescriptorContext : DbContext
    {
        public DescriptorContext(DbContextOptions<DescriptorContext> options) :
            base(options)
        {
        }

        public DbSet<DescriptorEntity> Descriptors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DescriptorEntityConfiguration());
        }
    }
}
