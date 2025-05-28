using CloudExchange.Domain.Providers;
using Microsoft.Extensions.Options;

namespace CloudExchange.API.Extensions.Startup
{
    public static class OptionsStartupExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection service, IConfiguration configuration) =>
            service.AddApplicationOptions(configuration);

        private static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration) =>
            services.AddSingleton(Options.Create(configuration.GetSection(nameof(DescriptorCredentialsHashProviderOptions))
                                                              .Get<DescriptorCredentialsHashProviderOptions>()!));
    }
}
