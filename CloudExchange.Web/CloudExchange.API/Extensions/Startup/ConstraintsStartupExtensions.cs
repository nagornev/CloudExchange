using CloudExchange.Domain.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ConstraintsStartupExtensions
    {
        public static IServiceCollection AddConstraints(this IServiceCollection services) =>
            services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = null)
                    .Configure<KestrelServerOptions>(options => options.Limits.MaxRequestBodySize = null)
                    .Configure<FormOptions>(options => options.MultipartBodyLengthLimit = DescriptorEntity.WeightMaximum);
    }
}
