using CloudExchange.Domain.Entities;
using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;
using System.IO;

namespace CloudExchange.Domain.Dto
{
    public class FileDto
    {
        public DescriptorEntity Descriptor { get; private set; }

        public Stream Data { get; private set; }

        private FileDto(DescriptorEntity descriptor,
                        Stream data)
        {
            Descriptor = descriptor;
            Data = data;
        }

        public static Result<FileDto> Create(DescriptorEntity descriptor, Stream data)
        {
            if (descriptor is null)
                return Result<FileDto>.Failure(Errors.InvalidArgument("The file descriptor can`t be null."));

            if (data is null)
                return Result<FileDto>.Failure(Errors.InvalidArgument("The file stream can`t be null."));

            return Result<FileDto>.Success(new FileDto(descriptor, data));
        }
    }
}
