using CloudExchange.Infrastructure.Services;
using CloudExchange.UseCases.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ServicesStartupExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddFileService()
                    .AddSchedulerService();

        private static IServiceCollection AddFileService(this IServiceCollection services) =>
            services.AddScoped<IUserFileService, FileService>()
                    .AddScoped<IServerFileService, FileService>();

        private static IServiceCollection AddSchedulerService(this IServiceCollection services) =>
            services.AddScoped<SchedulerService>()
                    .AddScoped<ISchedulerService, SchedulerServiceProxy>();
    }
}
