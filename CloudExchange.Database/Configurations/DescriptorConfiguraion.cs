using CloudExchange.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudExchange.Database.Configurations
{
    internal class DescriptorConfiguraion : IEntityTypeConfiguration<DescriptorEntity>
    {
        public void Configure(EntityTypeBuilder<DescriptorEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
