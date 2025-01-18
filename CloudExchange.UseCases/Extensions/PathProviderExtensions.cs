using CloudExchange.UseCases.Providers;
using System.IO;

namespace CloudExchange.UseCases.Extensions
{
    public static class PathProviderExtensions
    {
        public static string GetPath(this IPathProvider pathProvider)
        {
            return Path.Combine(pathProvider.GetBase(), pathProvider.GetDirectory());
        }
    }
}
