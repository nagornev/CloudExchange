using CloudExchange.Domain.Entities;
using System.Threading.Tasks;

namespace CloudExchange.Domain.Delegates
{
    public delegate Task<bool> TransactionDeleteDelegate(DescriptorEntity descriptor);
}
