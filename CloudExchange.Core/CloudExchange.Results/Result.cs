using System;
using System.IO.IsolatedStorage;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudExchange.OperationResults
{
    [Serializable]
    public class Result
    {
        protected Result(bool isSuccess,
                         Error error = default)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        [JsonPropertyName("success")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public bool IsSuccess { get; }

        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
        public Error Error { get; }

        [JsonIgnore]
        public bool IsFailure => !IsSuccess;

        public static Result Success()
        {
            return new Result(isSuccess: true);
        }

        public static Result Failure(Error error)
        {
            return new Result(isSuccess: false,
                              error: error);
        }

        #region Overrides

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        #endregion
    }
}
