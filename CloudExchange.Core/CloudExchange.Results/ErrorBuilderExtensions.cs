using System.Net;

namespace CloudExchange.OperationResults
{
    public static class ErrorBuilderExtensions
    {
        public static ErrorBuilder InternalServer(this ErrorBuilder builder,
                                                  string message)
        {
            builder.SetStatus(HttpStatusCode.InternalServerError)
                   .SetKey(ErrorKeys.InternalServer)
                   .SetMessage(message);

            return builder;
        }

        public static ErrorBuilder NullOrEmpty(this ErrorBuilder builder,
                                               string message,
                                               string field = default)
        {
            builder.SetStatus(HttpStatusCode.BadRequest)
                   .SetKey(ErrorKeys.NullOrEmpty)
                   .SetMessage(message)
                   .UseFactory((s, k, m) => new ErrorField(s, k, m, field));

            return builder;
        }

        public static ErrorBuilder InvalidArgument(this ErrorBuilder builder,
                                                   string message,
                                                   string field = default)
        {
            builder.SetStatus(HttpStatusCode.BadRequest)
                   .SetKey(ErrorKeys.InvalidArgument)
                   .SetMessage(message)
                   .UseFactory((s, k, m) => new ErrorField(s, k, m, field));

            return builder;
        }

        public static ErrorBuilder Operation(this ErrorBuilder builder,
                                             string message)
        {
            builder.SetStatus(HttpStatusCode.BadRequest)
                   .SetKey(ErrorKeys.Operation)
                   .SetMessage(message);

            return builder;
        }
    }
}
