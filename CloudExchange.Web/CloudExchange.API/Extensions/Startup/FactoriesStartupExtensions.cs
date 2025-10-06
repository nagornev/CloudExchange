using CloudExchange.Application.Abstractions.Factories;
using CloudExchange.Application.Factories;

namespace CloudExchange.API.Extensions.Startup
{
    public static class FactoriesStartupExtensions
    {
        public static IServiceCollection AddFactories(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IDescriptorFactory, DescriptorFactory>()
                           .AddSingleton<ISaltFactory, SaltFactory>();
        }
    }
}
