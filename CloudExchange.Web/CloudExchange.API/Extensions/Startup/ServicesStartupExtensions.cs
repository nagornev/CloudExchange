using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Abstractions.Services.Features.Files;
using CloudExchange.Application.Services;
using CloudExchange.Application.Services.Features.Files;
using CloudExchange.HangfireScheduler.Services;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ServicesStartupExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddFeatureServices();

        private static IServiceCollection AddFeatureServices(this IServiceCollection services) =>
            services.AddScoped<IDeleteFileByServerService, DeleteFileByServerService>()
                    .AddScoped<IDeleteFileScheduleService, DeleteFileScheduleService>();
    }
}
