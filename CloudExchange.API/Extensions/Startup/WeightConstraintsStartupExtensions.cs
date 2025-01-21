using CloudExchange.Domain.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace CloudExchange.API.Extensions.Startup
{
    public static class WeightConstraintsStartupExtensions
    {
        public static IServiceCollection AddConstraints(this IServiceCollection services) =>
            services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = null)
                    .Configure<KestrelServerOptions>(options => options.Limits.MaxRequestBodySize = null)
                    .Configure<FormOptions>(options => options.MultipartBodyLengthLimit = Descriptor.WeightMaximum);
    }
}
