using CloudExchange.Infrastructure.Providers;
using Microsoft.Extensions.Options;

namespace CloudExchange.API.Extensions.Startup
{
    public static class OptionsStartupExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration) =>
            services.AddSingleton(Options.Create(configuration.GetSection(nameof(HashProviderOptions))
                                                              .Get<HashProviderOptions>()!));
    }
}
