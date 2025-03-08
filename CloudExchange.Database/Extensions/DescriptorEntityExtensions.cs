using CloudExchange.Database.Entities;
using CloudExchange.OperationResults;
using Descriptor = CloudExchange.Domain.Models.Descriptor;

namespace CloudExchange.Database.Extensions
{
    public static class DescriptorEntityExtensions
    {
        public static Result<Descriptor> GetDomain(this DescriptorEntity descriptorEntity)
        {
            return Descriptor.Constructor(descriptorEntity.Id,
                                          descriptorEntity.Name,
                                          descriptorEntity.Path,
                                          descriptorEntity.Weight,
                                          descriptorEntity.Uploaded,
                                          descriptorEntity.Lifetime,
                                          descriptorEntity.Root,
                                          descriptorEntity.Download);
        }

        public static Result<IEnumerable<Descriptor>> GetDomain(this IReadOnlyCollection<DescriptorEntity> descriptorEntities)
        {
            List<Descriptor> descriptors = new List<Descriptor>();

            foreach (DescriptorEntity descriptorEntity in descriptorEntities)
            {
                Result<Descriptor> descriptorResult = descriptorEntity.GetDomain();

                if (!descriptorResult.Success)
                    return Result<IEnumerable<Descriptor>>.Failure(descriptorResult.Error);

                descriptors.Add(descriptorResult.Content);
            }

            return Result<IEnumerable<Descriptor>>.Successful(descriptors);
        }

        public static Result<IEnumerable<Descriptor>> GetDomain(this IEnumerable<DescriptorEntity> descriptorEntities)
        {
            return Result<IEnumerable<Descriptor>>.Successful(GetEnumerable(descriptorEntities));
        }

        public static Result<IAsyncEnumerable<Descriptor>> GetDomain(this IAsyncEnumerable<DescriptorEntity> descriptorEntities)
        {
            return Result<IAsyncEnumerable<Descriptor>>.Successful(GetAsyncEnumerable(descriptorEntities));
        }

        private static IEnumerable<Descriptor> GetEnumerable(IEnumerable<DescriptorEntity> descriptorEntities)
        {
            foreach (DescriptorEntity descriptorEntity in descriptorEntities)
            {
                yield return descriptorEntity.GetDomain()
                                             .Content;   
            }
        }

        private static async IAsyncEnumerable<Descriptor> GetAsyncEnumerable(IAsyncEnumerable<DescriptorEntity> descriptorEntities)
        {
            await foreach(var descriptorEntity in descriptorEntities)
            {
                yield return descriptorEntity.GetDomain()
                                             .Content;
            } 
        }
    }
}
