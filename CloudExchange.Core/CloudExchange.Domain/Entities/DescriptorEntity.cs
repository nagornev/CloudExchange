using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.ValueObjects;
using CloudExchange.OperationResults;
using System;
using System.Threading.Tasks;

namespace CloudExchange.Domain.Entities
{
    public class DescriptorEntity
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

        public DescriptorEntity(Guid id,
                                string name,
                                string path,
                                long weight,
                                long uploaded,
                                int lifetime,
                                DescriptorCredentialsValueObject credentials)
        {
            Id = id;
            Name = name;
            Path = path;
            Weight = weight;
            Uploaded = uploaded;
            Lifetime = lifetime;
            Credentials = credentials;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Path { get; private set; }

        public long Weight { get; private set; }

        public long Uploaded { get; private set; }

        public int Lifetime { get; private set; }

        public DescriptorCredentialsValueObject Credentials { get; private set; }

        private DescriptorEntity() { }

        public static Result<DescriptorEntity> Constructor(Guid id,
                                                           string name,
                                                           string path,
                                                           long weight,
                                                           long uploaded,
                                                           int lifetime,
                                                           DescriptorCredentialsValueObject credentials)
        {
            if (id == Guid.Empty)
                return Result<DescriptorEntity>.Failure(error => error.NullOrEmpty("The file ID can`t be null or empty."));

            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrWhiteSpace(name))
                return Result<DescriptorEntity>.Failure(error => error.NullOrEmpty("The file name can`t be null or empty."));

            if (string.IsNullOrEmpty(path) ||
                string.IsNullOrWhiteSpace(path))
                return Result<DescriptorEntity>.Failure(error => error.NullOrEmpty("The file path can`t be null or empty."));

            if (weight > WeightMaximum)
                return Result<DescriptorEntity>.Failure(error => error.InvalidArgument($"The file weight can`t be more than {WeightMaximum} KB."));

            if (!(lifetime >= LifetimeMinumum && lifetime <= LifetimeMaximum))
                return Result<DescriptorEntity>.Failure(error => error.InvalidArgument($"The file lifetime can`t be less than {LifetimeMinumum} and more than {LifetimeMaximum} seconds."));

            return Result<DescriptorEntity>.Successful(new DescriptorEntity(id,
                                                                            name,
                                                                            path,
                                                                            weight,
                                                                            uploaded,
                                                                            lifetime,
                                                                            credentials));
        }

        public static Result<DescriptorEntity> New(string name,
                                                   string path,
                                                   long weight,
                                                   long uploaded,
                                                   int lifetime,
                                                   string download = null,
                                                   string root = null,
                                                   IDescriptorCredentialsHashProvider hashProvider = null)
        {
            Result<DescriptorCredentialsValueObject> descriptorCredentialsResult = DescriptorCredentialsValueObject.Constructor(download, root, hashProvider);

            return descriptorCredentialsResult.Success ?
                        Constructor(Guid.NewGuid(),
                                    name,
                                    path,
                                    weight,
                                    uploaded,
                                    lifetime,
                                    descriptorCredentialsResult.Content) :
                        Result<DescriptorEntity>.Failure(descriptorCredentialsResult);
        }
    }
}
