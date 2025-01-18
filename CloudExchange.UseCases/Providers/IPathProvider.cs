namespace CloudExchange.UseCases.Providers
{
    public interface IPathProvider
    {
        string GetBase();

        string GetDirectory();
    }
}
