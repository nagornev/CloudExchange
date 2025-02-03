using Microsoft.AspNetCore.Cors.Infrastructure;

namespace CloudExchange.API.Extensions.Startup
{
    public static class CorsStartupExtensions
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration) =>
            services.AddCors(options => options.AddLocalhostPolicy());

        private static void AddLocalhostPolicy(this CorsOptions options) =>
            options.AddPolicy(options.DefaultPolicyName, policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                      .WithHeaders("Content-Disposition")
                      .WithMethods("GET", "POST", "DELETE")
                      .WithExposedHeaders("Content-Disposition")
                      .AllowCredentials();
            });
    }
}
