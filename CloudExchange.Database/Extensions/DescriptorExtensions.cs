using CloudExchange.Database.Entities;
using Descriptor = CloudExchange.Domain.Models.Descriptor;

namespace CloudExchange.Database.Extensions
{
    public static class DescriptorExtensions
    {
        public static DescriptorEntity GetEntity(this Descriptor file)
        {
            return new DescriptorEntity()
            {
                Id = file.Id,
                Name = file.Name,
                Path = file.Path,
                Weight = file.Weight,
                Uploaded = file.Uploaded,
                Lifetime = file.Lifetime,
                Root = file.Root,
                Download = file.Download,
            };
        }
    }
}
