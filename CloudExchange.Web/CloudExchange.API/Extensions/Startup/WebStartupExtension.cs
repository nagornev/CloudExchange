using CloudExchange.Domain.Aggregates;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using OperationResults;

namespace CloudExchange.API.Extensions.Startup
{
    public static class WebStartupExtension
    {
        public static IServiceCollection AddWeb(this IServiceCollection services) =>
            services.AddCors()
                    .AddControllers()
                    .AddValidators()
                    .AddConfigures();

        private static IServiceCollection AddCors(this IServiceCollection services)
        {
            return services.AddCors(options => options
                           .AddPolicy(options.DefaultPolicyName, policy => policy
                                     .WithOrigins("http://localhost:7001", "http://localhost:3000")
                                     .WithHeaders("Content-Disposition", "Content-Type")
                                     .WithMethods("GET", "POST", "DELETE", "PUT", "PATCH")
                                     .WithExposedHeaders("Content-Disposition")
                                     .AllowCredentials()));
        }

        private static IServiceCollection AddControllers(this IServiceCollection services)
        {
            return services.AddControllers(null)
                    .Services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true)
                           .AddValidatorsFromAssemblyContaining<Program>();
        }

        private static IServiceCollection AddConfigures(this IServiceCollection services)
        {
            return services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = null)
                           .Configure<KestrelServerOptions>(options => options.Limits.MaxRequestBodySize = null)
                           .Configure<FormOptions>(options => options.MultipartBodyLengthLimit = DescriptorAggregate.WeightMaximum)
                           .Configure<ApiBehaviorOptions>(options =>
                            options.InvalidModelStateResponseFactory = (context) =>
                            {
                                var entry = context.ModelState.FirstOrDefault(x => x.Value?.Errors.Count > 0);

                                var field = entry.Key;
                                var message = entry.Value!
                                                   .Errors.First()
                                                   .ErrorMessage;

                                return new BadRequestObjectResult(Result.Failure(ResultError.InvalidArgument(message)));
                            });
        }

    }
}
