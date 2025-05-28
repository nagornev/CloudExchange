using CloudExchange.Application.Abstractions.Services.Features.Files;
using CloudExchange.Application.Services.Features.Files;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.EntitiyFrameworkCore.Contexts;
using CloudExchange.EntitiyFrameworkCore.Repositories;
using CloudExchange.FileSystem.Repositories;
using CloudExchange.Hangfire.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.API.Extensions.Startup
{
    public static class InfrastructureStartupExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
             services.AddContexts(configuration)
                     .AddRepositories()
                     .AddFeatureServices();

        private static IServiceCollection AddContexts(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<DescriptorContext>(options => options.UseMySQL(configuration.GetConnectionString(nameof(DescriptorContext))!))

                    .AddHangfire(h => h.UseSqlServerStorage(configuration.GetConnectionString("HangfireContext")))
                    .AddHangfireServer();

        private static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddScoped<IDescriptorRepository, DescriptorRepository>()
                    .Decorate<IDescriptorRepository, DescriptorRepositoryExceptionDecorator>()
                    .AddScoped<IDataRepository, DataRepository>()
                    .Decorate<IDataRepository, DataRepositoryExceptionDecorator>();

        private static IServiceCollection AddFeatureServices(this IServiceCollection services) =>
            services.AddScoped<IDeleteFileScheduleService, DeleteFileScheduleService>();
    }
}
