using CloudExchange.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ContextsStartupExtensions
    {
        public static IServiceCollection AddContexts(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<DescriptorContext>(options => options.UseMySQL(configuration.GetConnectionString(nameof(DescriptorContext))));
    }
}
