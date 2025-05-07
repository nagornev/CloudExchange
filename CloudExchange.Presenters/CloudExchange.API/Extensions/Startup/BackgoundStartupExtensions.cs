using CloudExchange.API.Backgrounds;

namespace CloudExchange.API.Extensions.Startup
{
    public static class BackgoundStartupExtensions
    {
        public static IServiceCollection AddBackgrounds(this IServiceCollection services) =>
            services.AddHostedService<DeleteScheduler>();
    }
}
