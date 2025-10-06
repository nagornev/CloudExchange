using System;
using System.Text.Json.Serialization;

namespace OperationResults
{
    [Serializable]
    public class ResultError
    {
        public static ResultError NullOrEmpty(string message) => new ResultError(ResultErrorTypes.NullOrEmpty, message);

        public static ResultError InvalidArgument(string message) => new ResultError(ResultErrorTypes.InvalidArgument, message);

        public static ResultError NotFound(string message) => new ResultError(ResultErrorTypes.NotFound, message);

        public static ResultError Interrupted(string message) => new ResultError(ResultErrorTypes.Interrupted, message);

        public static ResultError TransactionFailed(string message) => new ResultError(ResultErrorTypes.Transaction, message);

        public static ResultError InvalidDownload(string message) => new ResultError(ResultErrorTypes.InvalidDownload, message);

        public static ResultError InvalidRoot(string message) => new ResultError(ResultErrorTypes.InvalidRoot, message);

        public static ResultError ServiceUnavailable(string message) => new ResultError(ResultErrorTypes.ServiceUnavailable, message);

        public static ResultError Already(string message) => new ResultError(ResultErrorTypes.Already, message);

        public ResultError(int type,
                     string message)
        {
            Type = type;
            Message = message;
        }

        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Type { get; }

        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Message { get; }
    }
}
