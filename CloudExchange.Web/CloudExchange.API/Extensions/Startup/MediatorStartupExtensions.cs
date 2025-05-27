using CloudExchange.Application;

namespace CloudExchange.API.Extensions.Startup
{
    public static class MediatorStartupExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services) =>
            services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
    }
}
