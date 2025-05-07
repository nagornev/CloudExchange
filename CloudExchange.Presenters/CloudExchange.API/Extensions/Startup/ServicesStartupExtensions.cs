using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Services;
using CloudExchange.HangfireScheduler.Services;

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
            services.AddScoped<ISchedulerService, SchedulerService>();
    }
}
