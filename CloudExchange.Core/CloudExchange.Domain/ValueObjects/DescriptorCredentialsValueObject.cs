using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;

namespace CloudExchange.Domain.ValueObjects
{
    public class DescriptorCredentialsValueObject
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

        public DescriptorCredentialsValueObject(string download,
                                                string root)
        {
            Download = download;
            Root = root;
        }

        private DescriptorCredentialsValueObject() { }

        public string Download { get; private set; }

        public string Root { get; private set; }

        public static Result<DescriptorCredentialsValueObject> Create(string download,
                                                                      string root,
                                                                      IDescriptorCredentialsHashProvider hashProvider)
        {
            if (!string.IsNullOrEmpty(root) &&
                !string.IsNullOrWhiteSpace(root) &&
                !(root.Length >= RootMinimumLenght && root.Length <= RootMaximumLenght))
                return Result<DescriptorCredentialsValueObject>.Failure(Errors.InvalidArgument($"The root password can`t be less than {RootMinimumLenght} and more than {RootMaximumLenght} symbols."));

            if (!string.IsNullOrEmpty(download) &&
                !string.IsNullOrWhiteSpace(download) &&
                !(download.Length >= DownloadMinimumLenght && download.Length <= DownloadMaximumLenght))
                return Result<DescriptorCredentialsValueObject>.Failure(Errors.InvalidArgument($"The download password can`t be less than {DownloadMinimumLenght} and more than {DownloadMaximumLenght} symbols."));

            if ((!string.IsNullOrEmpty(root) || !string.IsNullOrEmpty(download)) &&
                hashProvider == null)
                return Result<DescriptorCredentialsValueObject>.Failure(Errors.InvalidArgument("The hash provider can`t be null, if root or download password not null or empty."));

            return Result<DescriptorCredentialsValueObject>.Success(
                        new DescriptorCredentialsValueObject(!string.IsNullOrEmpty(download) ? hashProvider.Hash(download) : null,
                                                             !string.IsNullOrEmpty(root) ? hashProvider.Hash(root) : null));
        }
    }
}
