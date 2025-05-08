using CloudExchange.EntitiyFrameworkCorePersistence.Contexts;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ContextsStartupExtensions
    {
        public static IServiceCollection AddContexts(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDescriptorContext(configuration);
                    //.AddHangfireContext(configuration);


        private static IServiceCollection AddDescriptorContext(this IServiceCollection services, IConfiguration configuration)=>
            services.AddDbContext<DescriptorContext>(options => options.UseMySQL(configuration.GetConnectionString(nameof(DescriptorContext))!));

        private static IServiceCollection AddHangfireContext(this IServiceCollection services, IConfiguration configuration)=>
            services.AddHangfire(h => h.UseSqlServerStorage(configuration.GetConnectionString("HangfireContext")))
                    .AddHangfireServer();
    }
}
