using CloudExchange.Application.Options;
using CloudExchange.Messaging.Options;
using CloudExchange.Persistence.Options;
using Microsoft.Extensions.Options;

namespace CloudExchange.API.Extensions.Startup
{
    public static class OptionsStartupExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection service, IConfiguration configuration)
        {
            return service.AddApplicationOptions(configuration)
                          .AddInfrastructureOptions(configuration);
        }

        private static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton(Options.Create(configuration.GetSection(nameof(DescriptorCredentialsHashOptions))
                                                                     .Get<DescriptorCredentialsHashOptions>()!))
                           .AddSingleton(Options.Create(configuration.GetSection(nameof(DescriptorOptions))
                                                                     .Get<DescriptorOptions>()!))
                           .AddSingleton(Options.Create(configuration.GetSection(nameof(SaltOptions))
                                                                     .Get<SaltOptions>()!));
        }


        private static IServiceCollection AddInfrastructureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton(Options.Create(configuration.GetSection(nameof(RabbitMessageBrokerOptions))
                                                                     .Get<RabbitMessageBrokerOptions>()!))
                           .AddSingleton(Options.Create(configuration.GetSection(nameof(StorageOptions))
                                                                     .Get<StorageOptions>()!))
                           .AddSingleton(Options.Create(configuration.GetSection(nameof(AmazonS3Options))
                                                                     .Get<AmazonS3Options>()!));
        }
    }
}
