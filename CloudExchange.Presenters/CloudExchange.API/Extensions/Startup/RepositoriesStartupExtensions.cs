using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.EntitiyFrameworkCorePersistence.Repositories;
using CloudExchange.FileSystemPersistence.Repositories;

namespace CloudExchange.API.Extensions.Startup
{
    public static class RepositoriesStartupExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddDesciptoryRepository()
                    .AddDataRepository();

        private static IServiceCollection AddDesciptoryRepository(this IServiceCollection services) =>
            services.AddScoped<IDescriptorRepository, DescriptorRepository>()
                    .Decorate<IDescriptorRepository, DescriptorRepositoryExceptionDecorator>();

        private static IServiceCollection AddDataRepository(this IServiceCollection services) =>
            services.AddScoped<IDataRepository, DataRepository>()
                    .Decorate<IDataRepository, DataRepositoryExceptionDecorator>();
    }
}
