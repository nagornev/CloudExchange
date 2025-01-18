using CloudExchange.OperationResults;
using System.IO;

namespace CloudExchange.Domain.Models
{
    public class File
    {
        public Descriptor Descriptor { get; private set; }

        public Stream Data { get; private set; }

        private File(Descriptor descriptor, Stream data)
        {
            Descriptor = descriptor;
            Data = data;
        }

        public static Result<File> Constructor(Descriptor descriptor, Stream data)
        {
            if (descriptor is null)
                return Result<File>.Failure(error => error.InvalidArgument("The file descriptor can`t be null."));

            if (data is null)
                return Result<File>.Failure(error => error.InvalidArgument("The file stream can`t be null."));

            return Result<File>.Successful(new File(descriptor, data));
        }

        public static Result<File> New(Descriptor descriptor, Stream data)
        {
            return Constructor(descriptor, data);
        }
    }
}
