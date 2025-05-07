using CloudExchange.Application.Abstractions.Providers;

namespace CloudExchange.Application.Extensions
{
    internal static class PathProviderExtensions
    {
        public static string GetPath(this IPathProvider pathProvider)
        {
            return Path.Combine(pathProvider.GetBase(), pathProvider.GetDirectory());
        }
    }
}
