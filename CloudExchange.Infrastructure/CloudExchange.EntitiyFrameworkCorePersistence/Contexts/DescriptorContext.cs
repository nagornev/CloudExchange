using CloudExchange.Domain.Entities;
using CloudExchange.EntitiyFrameworkCorePersistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.EntitiyFrameworkCorePersistence.Contexts
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
