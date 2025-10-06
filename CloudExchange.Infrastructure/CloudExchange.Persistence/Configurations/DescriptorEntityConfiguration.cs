using CloudExchange.Domain.Aggregates;
using CloudExchange.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudExchange.Persistence.Configurations
{
    internal class DescriptorEntityConfiguration : IEntityTypeConfiguration<DescriptorAggregate>
    {
        public void Configure(EntityTypeBuilder<DescriptorAggregate> builder)
        {
            builder.HasKey(d => d.Id);

            builder.OwnsOne(d => d.Credentials, dc =>
            {
                dc.Property(c => c.Salt)
                  .HasColumnName(nameof(DescriptorCredentialsValueObject.Salt));

                dc.Property(c => c.DownloadHash)
                  .HasColumnName(nameof(DescriptorCredentialsValueObject.DownloadHash));

                dc.Property(c => c.RootHash)
                  .HasColumnName(nameof(DescriptorCredentialsValueObject.RootHash));
            });
        }
    }
}
