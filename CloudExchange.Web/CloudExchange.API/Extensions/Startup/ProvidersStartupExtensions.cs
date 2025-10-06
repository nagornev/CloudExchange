using CloudExchange.API.Abstractions.Providers;
using CloudExchange.API.Providers;
using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Providers;
using CloudExchange.Messaging.Abstractions.Providers;
using CloudExchange.Messaging.Providers;
using CloudExchange.Security.Providers;
using OperationResults;
using TimeProvider = CloudExchange.Application.Providers.TimeProvider;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ProvidersStartupExtensions
    {
        public static IServiceCollection AddProviders(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddApplicationProviders()
                           .AddInfrastructureProviders()
                           .AddPresenterProviders();
        }

        private static IServiceCollection AddApplicationProviders(this IServiceCollection services)
        {
            return services.AddSingleton<IStorageKeyProvider, StorageKeyProvider>()
                           .AddSingleton<ITimeProvider, TimeProvider>();
        }

        private static IServiceCollection AddInfrastructureProviders(this IServiceCollection services)
        {
            return services.AddScoped<IMessageContractProvider, DescriptorDeletedMessageContractProvider>()
                           .AddScoped<IMessageContractsProvider, MessageContractsProvider>()
                           .AddSingleton<IDescriptorCredentialsHashProvider, DescriptorCredentialsHashProvider>();
        }

        private static IServiceCollection AddPresenterProviders(this IServiceCollection services)
        {
            return services.AddSingleton<IResultProvider>(new ResultProviderBuilder()
                                                        .UseSuccess((options) => options
                                                            .UseNocontentFactory(result => Results.Ok(result)))
                                                        .UseFailed((options) => options
                                                            .UseFactory(result => result.Error.Type switch
                                                            {
                                                                ResultErrorTypes.InvalidArgument => Results.BadRequest(result),
                                                                ResultErrorTypes.NullOrEmpty => Results.BadRequest(result),
                                                                ResultErrorTypes.NotFound => Results.NotFound(result),
                                                                ResultErrorTypes.Interrupted => Results.NoContent(),
                                                                ResultErrorTypes.InvalidRoot => Results.Unauthorized(),
                                                                ResultErrorTypes.InvalidDownload => Results.Unauthorized(),
                                                                ResultErrorTypes.InvalidOperation => Results.StatusCode(500),
                                                                ResultErrorTypes.ServiceUnavailable => Results.StatusCode(500),
                                                                ResultErrorTypes.Transaction => Results.StatusCode(500),

                                                                _ => Results.BadRequest(result),
                                                            }))
                                                        .Build());

        }
    }
}
