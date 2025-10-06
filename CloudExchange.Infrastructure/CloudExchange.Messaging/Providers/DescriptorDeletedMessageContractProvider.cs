using CloudExchange.Domain.Events;
using MessageContracts;

namespace CloudExchange.Messaging.Providers
{
    public class DescriptorDeletedMessageContractProvider : MessageContractProvider<DescriptorDeletedDomainEvent>
    {
        public override Task<dynamic> Create(DescriptorDeletedDomainEvent domainEvent)
        {
            return Task.FromResult<dynamic>(new DescriptorDeletedMessageContract(domainEvent.AggregateId, domainEvent.Name, domainEvent.Upload));
        }
    }
}
