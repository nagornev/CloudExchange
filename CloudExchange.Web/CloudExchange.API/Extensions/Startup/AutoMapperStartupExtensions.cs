using CloudExchange.Application;

namespace CloudExchange.API.Extensions.Startup
{
    public static class AutoMapperStartupExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services) =>
             services.AddAutoMapper(typeof(AssemblyMarker).Assembly);
    }
}
