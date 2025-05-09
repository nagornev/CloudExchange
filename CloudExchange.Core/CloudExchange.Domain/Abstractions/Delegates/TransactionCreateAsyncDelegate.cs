using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using System.Threading;
using System.Threading.Tasks;

namespace CloudExchange.Domain.Abstractions.Delegates
{
    public delegate Task<Result> TransactionCreateAsyncDelegate(DescriptorEntity descriptor,
                                                                CancellationToken cancellation = default);
}
