using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using System.Threading.Tasks;

namespace CloudExchange.UseCases.Delegates
{
    public delegate Task<Result> TransactionCreateDelegate(Descriptor descriptor);
}
