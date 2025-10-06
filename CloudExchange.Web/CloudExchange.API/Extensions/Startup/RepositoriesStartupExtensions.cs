using CloudExchange.Application.Abstractions.Repositories;
using CloudExchange.Domain.Aggregates;
using CloudExchange.Persistence.Abstractions.Repositories;
using CloudExchange.Persistence.Contexts;
using CloudExchange.Persistence.Repositories;
using DDD.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CloudExchange.API.Extensions.Startup
{
    public static class RepositoriesStartupExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<DescriptorContext>(options => options.UseNpgsql(configuration.GetConnectionString(nameof(DescriptorContext))!))
                           .AddDescriptorRepository()
                           .AddOutboxtRepository()
                           .AddStorageRepository()
                           .AddScoped<IUnitOfWork, UnitOfWork>();

        }

        private static IServiceCollection AddDescriptorRepository(this IServiceCollection services)
        {
            return services.AddScoped<IRepository<DescriptorAggregate>, DescriptorRespository>()
                           .AddScoped<IRepositoryReader<DescriptorAggregate>>(x => x.GetRequiredService<IRepository<DescriptorAggregate>>())
                           .AddScoped<IRepositoryWriter<DescriptorAggregate>>(x => x.GetRequiredService<IRepository<DescriptorAggregate>>());
        }
        
        private static IServiceCollection AddOutboxtRepository(this IServiceCollection services)
        {
            return services.AddScoped<IOutboxRepository, OutboxRepository>();
        }

        private static IServiceCollection AddStorageRepository(this IServiceCollection services)
        {
            return services.AddScoped<IStorageRepository, StorageRepository>();
        }
    }
}
