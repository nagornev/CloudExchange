using CloudExchange.Infrastructure.Repositories;
using CloudExchange.UseCases.Repositories;

namespace CloudExchange.API.Extensions.Startup
{
    public static class RepositoriesStartupExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddDesciptoryRepository()
                    .AddStorageRepository();

        private static IServiceCollection AddDesciptoryRepository(this IServiceCollection services) =>
            services.AddScoped<DescriptorRepository>()
                    .AddScoped<IDescriptorRepository, DescriptorRepositoryProxy>();

        private static IServiceCollection AddStorageRepository(this IServiceCollection services) =>
            services.AddScoped<StorageRepository>()
                    .AddScoped<IStorageRepository, StorageRepository>();
    }
}
