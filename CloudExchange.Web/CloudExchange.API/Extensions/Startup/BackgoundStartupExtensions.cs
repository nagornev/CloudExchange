using CloudExchange.API.Backgrounds;
using CloudExchange.Backgrounds.Abstractions.Processors;
using CloudExchange.Backgrounds.Processors;
using Hangfire;
using Hangfire.PostgreSql;

namespace CloudExchange.API.Extensions.Startup
{
    public static class BackgoundStartupExtensions
    {
        private const string _connectionStringName = "HangfireContext";

        public static IServiceCollection AddBackgrounds(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddHangfire(options =>
                            {
                                options.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                       .UseSimpleAssemblyNameTypeSerializer()
                                       .UseRecommendedSerializerSettings()
                                       .UsePostgreSqlStorage(cfg => cfg.UseNpgsqlConnection(configuration.GetConnectionString(_connectionStringName)));
                            })
                           .AddHangfireServer()
                           .AddBackgroundProcessors()
                           .AddHostedService<BackgroundProccessorsStarter>();
        }

        private static IServiceCollection AddBackgroundProcessors(this IServiceCollection services)
        {
            return services.AddSingleton<IBackgroundProcessor, DeleteExpiredDescriptorsBackgroundProcessor>()
                           .AddSingleton<IBackgroundProcessor, OutboxBackgroundProcessor>();
        }

    }
}
