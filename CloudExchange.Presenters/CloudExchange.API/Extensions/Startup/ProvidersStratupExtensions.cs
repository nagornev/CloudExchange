using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Providers;
using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.Providers;
using TimeProvider = CloudExchange.Application.Providers.TimeProvider;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ProvidersStratupExtensions
    {
        public static IServiceCollection AddProviders(this IServiceCollection services) =>
            services.AddSingleton<ITimeProvider, TimeProvider>()
                    .AddSingleton<IPathProvider, PathProvider>()
                    .AddSingleton<IDescriptorCredentialsHashProvider, DescriptorCredentialsHashProvider>();
    }
}
