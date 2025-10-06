using DDD.Events;

namespace CloudExchange.Domain.Events
{
    public class DescriptorConfirmedDomainEvent : DomainEvent
    {
        public DescriptorConfirmedDomainEvent(Guid aggregateId)
            : base(aggregateId)
        {
        }
    }
}
