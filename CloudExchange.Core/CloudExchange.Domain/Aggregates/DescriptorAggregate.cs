using CloudExchange.Domain.Events;
using CloudExchange.Domain.ValueObjects;
using DDD.Primitives;
using OperationResults;

namespace CloudExchange.Domain.Aggregates
{
    public partial class DescriptorAggregate : AggregateRoot
    {
        /// <summary>
        /// Maximum file weight in bytes.
        /// </summary>
        public const long WeightMaximum = 10_737_418_240;

        /// <summary>
        /// Minumum file lifetime in seconds.
        /// </summary>
        public const int LifetimeMinumum = 600;

        /// <summary>
        /// Maximum file lifetime in seconds.
        /// </summary>
        public const int LifetimeMaximum = 604_800;

        private DescriptorAggregate(Guid id,
                                   string name,
                                   long weight,
                                   int lifetime,
                                   long createdAt,
                                   long expiresAt,
                                   DescriptorCredentialsValueObject credentials)
        {
            Id = id;
            Name = name;
            Weight = weight;
            Lifetime = lifetime;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
            Credentials = credentials;
        }

        public string? Upload { get; private set; }

        public string Name { get; private set; }

        public long Weight { get; private set; }

        public int Lifetime { get; private set; }

        public long CreatedAt { get; private set; }

        public long ExpiresAt { get; private set; }

        public bool Confirmed { get; private set; }

        public bool Deleted { get; private set; }

        public DescriptorCredentialsValueObject? Credentials { get; private set; }

        public static Result<DescriptorAggregate> Create(Guid id,
                                                         string name,
                                                         long weight,
                                                         int lifetime,
                                                         long createdAt,
                                                         long expiresAt,
                                                         DescriptorCredentialsValueObject? credentials = null)
        {
            if (id == Guid.Empty)
                return Result<DescriptorAggregate>.Failure(ResultError.NullOrEmpty("The file ID can`t be null or empty."));

            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrWhiteSpace(name))
                return Result<DescriptorAggregate>.Failure(ResultError.NullOrEmpty("The file name can`t be null or empty."));

            if (lifetime > LifetimeMaximum || lifetime < LifetimeMinumum)
                return Result<DescriptorAggregate>.Failure(ResultError.InvalidArgument($"The lifetime can`t be less than {LifetimeMinumum} and more than {LifetimeMaximum} seconds."));

            if (weight > WeightMaximum)
                return Result<DescriptorAggregate>.Failure(ResultError.InvalidArgument($"The file weight can`t be more than {WeightMaximum} KB."));

            if (!((expiresAt - createdAt) >= LifetimeMinumum && (expiresAt - createdAt) <= LifetimeMaximum))
                return Result<DescriptorAggregate>.Failure(ResultError.InvalidArgument($"The file expiresAt can`t be less than {LifetimeMinumum} and more than {LifetimeMaximum} seconds."));

            return Result<DescriptorAggregate>.Success(new DescriptorAggregate(id,
                                                                               name,
                                                                               weight,
                                                                               lifetime,
                                                                               createdAt,
                                                                               expiresAt,
                                                                               credentials));
        }

        public static Result<DescriptorAggregate> NewUnprotected(string name,
                                                                 long weight,
                                                                 int lifetime,
                                                                 long createdAt,
                                                                 long expiresAt)
        {
            return Create(Guid.NewGuid(),
                          name,
                          weight,
                          lifetime,
                          createdAt,
                          expiresAt,
                          default);
        }

        public static Result<DescriptorAggregate> NewProtected(string name,
                                                               long weight,
                                                               int lifetime,
                                                               long createdAt,
                                                               long expiresAt,
                                                               string salt,
                                                               string downloadHash,
                                                               string rootHash)
        {
            Result<DescriptorCredentialsValueObject> credentialsResult = DescriptorCredentialsValueObject.Create(salt, downloadHash, rootHash);

            return credentialsResult.IsSuccess ?
                   Create(Guid.NewGuid(),
                          name,
                          weight,
                          lifetime,
                          createdAt,
                          expiresAt,
                          credentialsResult.Content) :
                   Result<DescriptorAggregate>.Failure(credentialsResult.Error);
        }

        public bool IsValidAt(long timestamp)
        {
            if (!Confirmed || Deleted)
                return false;

            return ExpiresAt > timestamp;
        }

        public Result SetUpload(string upload)
        {
            if (!string.IsNullOrEmpty(Upload) ||
                !string.IsNullOrWhiteSpace(Upload))
                return Result.Failure(ResultError.Already("The descriptor upload is already set."));

            if (string.IsNullOrEmpty(upload) ||
                string.IsNullOrWhiteSpace(upload))
                return Result.Failure(ResultError.NullOrEmpty("The descriptor upload is null or empty."));

            Upload = upload;

            return Result.Success();
        }

        public Result Confirm()
        {
            if (Confirmed)
                return Result.Failure(ResultError.Already("This descriptor is already confirmed."));

            Confirmed = true;
            ExpiresAt = CreatedAt + Lifetime;
            Upload = null;

            return Result.Success();
        }

        public Result MarkAsDeleted()
        {
            if (Deleted)
                return Result.Failure(ResultError.Already("This descriptor is already marked as delete."));

            Deleted = true;

            AddDomainEvent(new DescriptorDeletedDomainEvent(Id, Name, Upload));

            return Result.Success();
        }
    }

    #region Ef

    public partial class DescriptorAggregate
    {
        private DescriptorAggregate() { }
    }

    #endregion
}
