using CloudExchange.OperationResults;
using Microsoft.AspNetCore.Mvc;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ControllersStartupExtensions
    {
        public static IMvcBuilder AddControllers(this IServiceCollection services, IConfiguration configuration) =>
            services.AddControllers()
                    .ConfigureInvalidResposeFactory();

        private static IMvcBuilder ConfigureInvalidResposeFactory(this IMvcBuilder builder) =>
            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var entry = context.ModelState.FirstOrDefault(x => x.Value?.Errors.Count > 0);

                    var field = entry.Key;
                    var message = entry.Value!
                                       .Errors.First()
                                       .ErrorMessage;

                    return new BadRequestObjectResult(Result.Failure(error => error.InvalidArgument(message, field)));
                };
            });
    }
}
