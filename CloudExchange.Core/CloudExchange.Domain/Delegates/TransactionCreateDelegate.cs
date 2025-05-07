using CloudExchange.Domain.Entities;
using System.Threading.Tasks;

namespace CloudExchange.Domain.Delegates
{
    public delegate Task<bool> TransactionCreateDelegate(DescriptorEntity descriptor);
}
