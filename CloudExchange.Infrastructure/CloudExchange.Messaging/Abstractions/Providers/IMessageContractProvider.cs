using DDD.Events;

namespace CloudExchange.Messaging.Abstractions.Providers
{
    public interface IMessageContractProvider
    {
        Type GetHandableType();

        Task<dynamic> Create(IDomainEvent domainEvent);
    }
}
