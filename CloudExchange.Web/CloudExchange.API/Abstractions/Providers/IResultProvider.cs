using OperationResults;

namespace CloudExchange.API.Abstractions.Providers
{
    public interface IResultProvider
    {
        IResult GetResult(Result result);

        IResult GetResult<T>(Result<T> result);
    }
}
