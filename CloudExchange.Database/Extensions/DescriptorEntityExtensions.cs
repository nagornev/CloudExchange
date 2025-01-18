using CloudExchange.Database.Entities;
using CloudExchange.OperationResults;
using Descriptor = CloudExchange.Domain.Models.Descriptor;

namespace CloudExchange.Database.Extensions
{
    public static class DescriptorEntityExtensions
    {
        public static Result<Descriptor> GetDomain(this DescriptorEntity fileEntity)
        {
            return Descriptor.Constructor(fileEntity.Id,
                                          fileEntity.Name,
                                          fileEntity.Path,
                                          fileEntity.Weight,
                                          fileEntity.Uploaded,
                                          fileEntity.Lifetime,
                                          fileEntity.Root,
                                          fileEntity.Download);
        }

        public static Result<IEnumerable<Descriptor>> GetDomain(this IEnumerable<DescriptorEntity> fileEntities)
        {
            List<Descriptor> files = new List<Descriptor>();

            foreach (DescriptorEntity fileEntity in fileEntities)
            {
                Result<Descriptor> fileResult = fileEntity.GetDomain();

                if (!fileResult.Success)
                    return Result<IEnumerable<Descriptor>>.Failure(fileResult.Error);

                files.Add(fileResult.Content);
            }

            return Result<IEnumerable<Descriptor>>.Successful(files);
        }
    }
}
