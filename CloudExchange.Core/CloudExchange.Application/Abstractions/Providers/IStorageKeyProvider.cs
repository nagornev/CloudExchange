namespace CloudExchange.Application.Abstractions.Providers
{
    public interface IStorageKeyProvider
    {
        string Get(Guid id, string name);

        Guid GetId(string key);
    }
}
