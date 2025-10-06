using DDD.Primitives;
using OperationResults;

namespace CloudExchange.Domain.ValueObjects
{
    public class DescriptorCredentialsValueObject : ValueObject
    {
        /// <summary>
        /// Minimum root password lenght.
        /// </summary>
        public const int RootMinimumLenght = 5;

        /// <summary>
        /// Maximum root password lenght.
        /// </summary>
        public const int RootMaximumLenght = 20;

        /// <summary>
        /// Minimum download password lenght.
        /// </summary>
        public const int DownloadMinimumLenght = 3;

        /// <summary>
        /// Maximum download password lenght.
        /// </summary>
        public const int DownloadMaximumLenght = 15;

        private DescriptorCredentialsValueObject(string salt,
                                                 string downloadHash,
                                                 string rootHash)
        {
            Salt = salt;
            DownloadHash = downloadHash;
            RootHash = rootHash;
        }



        public string? Salt { get; private set; }

        public string? DownloadHash { get; private set; }

        public string? RootHash { get; private set; }

        public static Result<DescriptorCredentialsValueObject> Create(string salt,
                                                                      string downloadHash,
                                                                      string rootHash)
        {
            if (string.IsNullOrEmpty(salt) ||
                string.IsNullOrWhiteSpace(salt))
                return Result<DescriptorCredentialsValueObject>.Failure(ResultError.NullOrEmpty("The salt can`t be null or empty."));

            return Result<DescriptorCredentialsValueObject>.Success(
                        new DescriptorCredentialsValueObject(salt,
                                                             downloadHash,
                                                             rootHash));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Salt;
            yield return DownloadHash;
            yield return RootHash;
        }

        private DescriptorCredentialsValueObject() 
        {
        }
    }
}
