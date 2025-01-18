using Hangfire;

namespace CloudExchange.API.Extensions.Startup
{
    public static class HangfireStratupExtensions
    {
        public static IServiceCollection AddHangfires(this IServiceCollection services, IConfiguration configuration) =>
            services.AddHangfire(h => h.UseSqlServerStorage(configuration.GetConnectionString("HangfireContext")))
                    .AddHangfireServer();
    }
}
