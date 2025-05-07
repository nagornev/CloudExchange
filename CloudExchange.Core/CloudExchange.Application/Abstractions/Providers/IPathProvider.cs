namespace CloudExchange.Application.Abstractions.Providers
{
    public interface IPathProvider
    {
        string GetBase();

        string GetDirectory();
    }
}
