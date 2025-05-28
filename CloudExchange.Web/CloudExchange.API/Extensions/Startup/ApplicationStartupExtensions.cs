using CloudExchange.Application;
using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Services.Features.Files;
using CloudExchange.Application.Providers;
using CloudExchange.Application.Services.Features.Files;
using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.Providers;
using TimeProvider = CloudExchange.Application.Providers.TimeProvider;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ApplicationStartupExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) =>
            services.AddMediator()
                    .AddMapper()
                    .AddProviders()
                    .AddFeatureServices();

        private static IServiceCollection AddMediator(this IServiceCollection services) =>
            services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));

        private static IServiceCollection AddMapper(this IServiceCollection services) =>
            services.AddAutoMapper(typeof(AssemblyMarker).Assembly);

        private static IServiceCollection AddProviders(this IServiceCollection services) =>
            services.AddSingleton<ITimeProvider, TimeProvider>()
                    .AddSingleton<IPathProvider, PathProvider>()
                    .AddSingleton<IDescriptorCredentialsHashProvider, DescriptorCredentialsHashProvider>();

        private static IServiceCollection AddFeatureServices(this IServiceCollection services) =>
            services.AddScoped<IDeleteFileByServerService, DeleteFileByServerService>();
    }
}
