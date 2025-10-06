namespace CloudExchange.Application.Abstractions.Services
{
    public interface IDeleteExpiredDescriptorsBackgroundService
    {
        Task DeleteAsync(CancellationToken cancellation = default);
    }
}
