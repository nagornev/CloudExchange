using CloudExchange.Domain.Entities;
using CloudExchange.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudExchange.EntitiyFrameworkCorePersistence.Configurations
{
    internal class DescriptorEntityConfiguration : IEntityTypeConfiguration<DescriptorEntity>
    {
        public void Configure(EntityTypeBuilder<DescriptorEntity> builder)
        {
            builder.HasKey(d => d.Id);

            builder.OwnsOne(d => d.Credentials, dc =>
            {
                dc.Property(c => c.Download)
                  .HasColumnName(nameof(DescriptorCredentialsValueObject.Download))
                  .HasMaxLength(DescriptorCredentialsValueObject.DownloadMaximumLenght);

                dc.Property(c => c.Root)
                  .HasColumnName(nameof(DescriptorCredentialsValueObject.Root))
                  .HasMaxLength(DescriptorCredentialsValueObject.RootMaximumLenght);
            });
        }
    }
}
