using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;

namespace CloudExchange.Application.Dto
{
    public class FileDto
    {
        public FileDto(DescriptorDto descriptor,
                       Stream data)
        {
            Descriptor = descriptor;
            Data = data;
        }

        public DescriptorDto Descriptor { get; }

        public Stream Data { get; }
    }
}
