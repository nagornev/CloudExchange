using CloudExchange.OperationResults;

namespace CloudExchange.Domain.Failures
{
    public class Errors
    {
        public static Error NullOrEmpty(string message) => new Error(ErrorTypes.NullOrEmpty, message);

        public static Error InvalidArgument(string message) => new Error(ErrorTypes.InvalidArgument, message);

        public static Error NotFound(string message) => new Error(ErrorTypes.NotFound, message);

        public static Error Interrupted(string message) => new Error(ErrorTypes.Interrupted, message);

        public static Error TransactionFailed(string message) => new Error(ErrorTypes.TransactionFailed, message);

        public static Error InvalidDownload(string message) => new Error(ErrorTypes.InvalidDownload, message);

        public static Error InvalidRoot(string message) => new Error(ErrorTypes.InvalidRoot, message);
    }
}
