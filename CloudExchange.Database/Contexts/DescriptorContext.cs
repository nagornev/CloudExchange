using CloudExchange.Database.Configurations;
using CloudExchange.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.Database.Contexts
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
            modelBuilder.ApplyConfiguration(new DescriptorConfiguraion());
        }
    }
}
