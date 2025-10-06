using CloudExchange.Application.Abstractions.Factories;
using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Options;
using CloudExchange.Domain.Aggregates;
using Microsoft.Extensions.Options;
using OperationResults;

namespace CloudExchange.Application.Factories
{
    public class DescriptorFactory : IDescriptorFactory
    {
        private readonly ISaltFactory _saltFactory;

        private readonly IDescriptorCredentialsHashProvider _descriptorCredentialsHashProvider;

        private readonly ITimeProvider _timeProvider;

        private readonly DescriptorOptions _descriptorOptions;

        public DescriptorFactory(ISaltFactory saltFactory,
                                 IDescriptorCredentialsHashProvider descriptorCredentialsHashProvider,
                                 ITimeProvider timeProvider,
                                 IOptions<DescriptorOptions> descriptorOptions)
        {
            _saltFactory = saltFactory;
            _descriptorCredentialsHashProvider = descriptorCredentialsHashProvider;
            _timeProvider = timeProvider;
            _descriptorOptions = descriptorOptions.Value;
        }

        public Result<DescriptorAggregate> Create(string name, long weight, int lifetime, string? root = null, string? download = null)
        {
            string salt = _saltFactory.Create();
            long createdAt = _timeProvider.NowUnix();
            long expiresAt = createdAt + lifetime;

            Result<DescriptorAggregate> descriptorResult = string.IsNullOrEmpty(root) && string.IsNullOrEmpty(download) ?
                                                           DescriptorAggregate.NewUnprotected(name,
                                                                                              weight,
                                                                                              lifetime,
                                                                                              createdAt,
                                                                                              expiresAt) :
                                                           DescriptorAggregate.NewProtected(name,
                                                                                            weight,
                                                                                            lifetime,
                                                                                            createdAt,
                                                                                            expiresAt,
                                                                                            salt,
                                                                                            _descriptorCredentialsHashProvider.Hash(download, salt),
                                                                                            _descriptorCredentialsHashProvider.Hash(root, salt));

            return descriptorResult;
        }
    }
}
