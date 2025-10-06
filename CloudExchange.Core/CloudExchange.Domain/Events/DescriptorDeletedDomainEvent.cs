using DDD.Events;

namespace CloudExchange.Domain.Events
{
    public class DescriptorDeletedDomainEvent : DomainEvent
    {
        public DescriptorDeletedDomainEvent(Guid aggregateId,
                                            string name,
                                            string upload)
            : base(aggregateId)
        {
            Name = name;
            Upload = upload;
        }

        public string Name { get; }
        public string Upload { get; }
    }
}
