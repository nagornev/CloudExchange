using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using System.Threading;
using System.Threading.Tasks;

namespace CloudExchange.Domain.Delegates
{
    public delegate Task<Result> TransactionDeleteAsyncDelegate(DescriptorEntity descriptor,
                                                                CancellationToken cancellation = default);
}
