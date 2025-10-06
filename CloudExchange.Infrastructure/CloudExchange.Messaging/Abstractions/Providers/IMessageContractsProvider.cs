using DDD.Events;

namespace CloudExchange.Messaging.Abstractions.Providers
{
    public interface IMessageContractsProvider
    {
        Task<dynamic> CreateAsync(IDomainEvent domainEvent);
    }
}
