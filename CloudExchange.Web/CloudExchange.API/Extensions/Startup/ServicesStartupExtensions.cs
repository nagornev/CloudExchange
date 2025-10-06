using CloudExchange.Application;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Services;
using CloudExchange.Messaging.Services;
using CloudExchange.Persistence.Services;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ServicesStartupExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddApplicationServices()
                           .AddInfrastructureServices()
                          
                           .AddMediator()
                           .AddMapper();
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services.AddScoped<IAbortUploadFileService, AbortUploadFileService>()
                           .AddScoped<IBeginUploadFileService, BeginUploadFileService>()
                           .AddScoped<ICompleteUploadFileService, CompleteUploadFileService>()
                           .AddScoped<IContinueUploadFileService, ContinueUploadFileService>()
                           .AddScoped<IDeleteExpiredDescriptorsBackgroundService, DeleteExpiredDescriptorsBackgroundService>()
                           .AddScoped<IDeleteFileService, DeleteFileService>()
                           .AddScoped<IDescriptorDeletedEventService, DescriptorDeletedEventService>()
                           .AddScoped<IDescriptorInitializeService, DescriptorInitializeService>()
                           .AddScoped<IDescriptorQueryService, DescriptorQueryService>()
                           .AddScoped<IDownloadFileService, DownloadFileService>()
                           .AddScoped<IFileUploadCompletedEventService, FileUploadCompletedEventService>();
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services.AddScoped<IOutboxService, OutboxService>()
                           .AddScoped<IPublishEventService, PublishEventService>();
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            return services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
        }

        private static IServiceCollection AddMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(AssemblyMarker).Assembly);
        }

    }
}
