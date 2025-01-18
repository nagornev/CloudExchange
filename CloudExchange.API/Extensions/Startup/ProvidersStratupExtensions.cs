using CloudExchange.Infrastructure.Providers;
using CloudExchange.UseCases.Providers;
using TimeProvider = CloudExchange.Infrastructure.Providers.TimeProvider;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ProvidersStratupExtensions
    {
        public static IServiceCollection AddProviders(this IServiceCollection services) =>
            services.AddSingleton<ITimeProvider, TimeProvider>()
                    .AddSingleton<IHashProvider, HashProvider>()
                    .AddSingleton<IPathProvider, PathProvider>();
    }
}
