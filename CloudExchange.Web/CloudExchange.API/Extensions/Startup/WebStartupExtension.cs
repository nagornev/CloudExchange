using CloudExchange.API.Abstractions.Providers;
using CloudExchange.API.Providers;
using CloudExchange.Application.Dto;
using CloudExchange.Domain.Entities;
using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net.Mime;

namespace CloudExchange.API.Extensions.Startup
{
    public static class WebStartupExtension
    {
        public static IServiceCollection AddWeb(this IServiceCollection services) =>
            services.AddCors()
                    .AddProviders()
                    .AddControllers()
                    .AddValidators()
                    .AddConfigures();

        private static IServiceCollection AddCors(this IServiceCollection services) =>
            services.AddCors(options => options
                    .AddPolicy(options.DefaultPolicyName, policy => policy
                              .WithOrigins("http://localhost:7001", "http://localhost:3000")
                              .WithHeaders("Content-Disposition", "Content-Type")
                              .WithMethods("GET", "POST", "DELETE")
                              .WithExposedHeaders("Content-Disposition")
                              .AllowCredentials()));

        private static IServiceCollection AddControllers(this IServiceCollection services) =>
            services.AddControllers(null)
                    .Services;

        private static IServiceCollection AddProviders(this IServiceCollection services) =>
            services.AddSingleton<IResultProvider>(new ResultProviderBuilder()
                                                        .UseSuccess((options) => options
                                                            .UseNocontentFactory(result => Results.Ok(result))
                                                            .AddContentFactory<FileDto>(result => Results.Stream(result.Content.Data,
                                                                                                                 MediaTypeNames.Application.Octet,
                                                                                                                 result.Content.Descriptor.Name)))
                                                        .UseFailed((options) => options
                                                            .UseFactory(result => result.Error.Type switch
                                                            {
                                                                ErrorTypes.InvalidArgument => Results.BadRequest(result),
                                                                ErrorTypes.NullOrEmpty => Results.BadRequest(result),
                                                                ErrorTypes.NotFound => Results.NotFound(result),
                                                                ErrorTypes.Interrupted => Results.NoContent(),
                                                                ErrorTypes.InvalidRoot => Results.Unauthorized(),
                                                                ErrorTypes.InvalidDownload => Results.Unauthorized(),
                                                                ErrorTypes.InvalidOperation => Results.StatusCode(500),
                                                                ErrorTypes.ServiceUnavailable => Results.StatusCode(500),

                                                                _ => Results.BadRequest(result),
                                                            }))
                                                        .Build());

        public static IServiceCollection AddValidators(this IServiceCollection services) =>
            services.AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true)
                    .AddValidatorsFromAssemblyContaining<Program>();

        private static IServiceCollection AddConfigures(this IServiceCollection services) =>
             services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = null)
                     .Configure<KestrelServerOptions>(options => options.Limits.MaxRequestBodySize = null)
                     .Configure<FormOptions>(options => options.MultipartBodyLengthLimit = DescriptorEntity.WeightMaximum)
                     .Configure<ApiBehaviorOptions>(options =>
                      options.InvalidModelStateResponseFactory = (context) =>
                      {
                          var entry = context.ModelState.FirstOrDefault(x => x.Value?.Errors.Count > 0);

                          var field = entry.Key;
                          var message = entry.Value!
                                             .Errors.First()
                                             .ErrorMessage;

                          return new BadRequestObjectResult(Result.Failure(Errors.InvalidArgument(message)));
                      });
    }
}
