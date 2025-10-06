using CloudExchange.Application.Abstractions.Validators;
using CloudExchange.Application.Validators;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ValidatorStartupExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IDescriptorCredentialsValidator, DescriptorCredentialsValidator>();
        }
    }
}
