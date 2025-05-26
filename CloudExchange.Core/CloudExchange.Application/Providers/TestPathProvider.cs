using CloudExchange.Application.Abstractions.Providers;

namespace CloudExchange.Application.Providers
{
    public class TestPathProvider : IPathProvider
    {
        public string GetBase()
        {
            return @"E:\Projects\02_WEB\CloudExchange\CloudExchange.Data\";
        }

        public string GetDirectory()
        {
            return @"Files\";
        }
    }
}
