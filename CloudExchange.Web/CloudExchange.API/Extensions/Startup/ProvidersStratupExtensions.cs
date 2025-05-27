using CloudExchange.API.Abstractions.Providers;
using CloudExchange.API.Providers;
using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Dto;
using CloudExchange.Application.Providers;
using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.Failures;
using CloudExchange.Domain.Providers;
using System.Net.Mime;
using TimeProvider = CloudExchange.Application.Providers.TimeProvider;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ProvidersStratupExtensions
    {
        public static IServiceCollection AddProviders(this IServiceCollection services) =>
            services.AddSingleton<ITimeProvider, TimeProvider>()
                    .AddSingleton<IPathProvider, TestPathProvider>()
                    .AddSingleton<IDescriptorCredentialsHashProvider, DescriptorCredentialsHashProvider>()
                    .AddResultProvider();

        private static IServiceCollection AddResultProvider(this IServiceCollection services) =>
            services.AddSingleton<IResultProvider>(new ResultProviderBuilder()
                                                        .UseSuccess((options) => options
                                                            .UseNocontentFactory(result => Results.Ok(result))
                                                            .AddContentFactory<FileDto>(result => Results.Stream(result.Content.Data,
                                                                                                                 MediaTypeNames.Application.Octet,
                                                                                                                 result.Content.Descriptor.Name)))
                                                        .UseFailed((options) => options
                                                            .UseFactory(result => result.Error.Type switch
                                                            {
                                                                ErrorTypes.InvalidArgument => Results.BadRequest(result),
                                                                ErrorTypes.NullOrEmpty => Results.BadRequest(result),
                                                                ErrorTypes.NotFound => Results.NotFound(result),
                                                                ErrorTypes.Interrupted => Results.NoContent(),
                                                                ErrorTypes.InvalidRoot => Results.Unauthorized(),
                                                                ErrorTypes.InvalidDownload => Results.Unauthorized(),
                                                                ErrorTypes.InvalidOperation => Results.StatusCode(500),
                                                                ErrorTypes.ServiceUnavailable => Results.StatusCode(500),

                                                                _ => Results.BadRequest(result),
                                                            }))
                                                        .Build());
    }
}
