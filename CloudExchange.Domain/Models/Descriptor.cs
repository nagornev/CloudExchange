using CloudExchange.OperationResults;
using System;

namespace CloudExchange.Domain.Models
{
    public class Descriptor
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

        public Descriptor(Guid id,
                          string name,
                          string path,
                          long weight,
                          long uploaded,
                          int lifetime,
                          string root = null,
                          string download = null)
        {
            Id = id;
            Name = name;
            Path = path;
            Weight = weight;
            Uploaded = uploaded;
            Lifetime = lifetime;
            Root = root;
            Download = download;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Path { get; private set; }

        public long Weight { get; private set; }

        public long Uploaded { get; private set; }

        public int Lifetime { get; private set; }

        public string Root { get; private set; }

        public string Download { get; private set; }

        public static Result<Descriptor> Constructor(Guid id,
                                                     string name,
                                                     string path,
                                                     long weight,
                                                     long uploaded,
                                                     int lifetime,
                                                     string root = null,
                                                     string download = null)
        {
            if (id == Guid.Empty)
                return Result<Descriptor>.Failure(error => error.NullOrEmpty("The file ID can`t be null or empty."));

            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrWhiteSpace(name))
                return Result<Descriptor>.Failure(error => error.NullOrEmpty("The file name can`t be null or empty."));

            if (string.IsNullOrEmpty(path) ||
                string.IsNullOrWhiteSpace(path))
                return Result<Descriptor>.Failure(error => error.NullOrEmpty("The file path can`t be null or empty."));

            if (weight > WeightMaximum)
                return Result<Descriptor>.Failure(error => error.InvalidArgument($"The file weight can`t be more than {WeightMaximum} KB."));

            if (!(lifetime >= LifetimeMinumum && lifetime <= LifetimeMaximum))
                return Result<Descriptor>.Failure(error => error.InvalidArgument($"The file lifetime can`t be less than {LifetimeMinumum} and more than {LifetimeMaximum} seconds."));

            return Result<Descriptor>.Successful(new Descriptor(id,
                                                                name,
                                                                path,
                                                                weight,
                                                                uploaded,
                                                                lifetime,
                                                                root,
                                                                download));
        }

        public static Result<Descriptor> New(string name,
                                             string path,
                                             long weight,
                                             long uploaded,
                                             int lifetime,
                                             string root = null,
                                             string download = null,
                                             Func<string, string> hashFactory = null)
        {

            if (!string.IsNullOrEmpty(root) &&
                !string.IsNullOrWhiteSpace(root) &&
                !(root.Length >= RootMinimumLenght && root.Length <= RootMaximumLenght))
                return Result<Descriptor>.Failure(error => error.InvalidArgument($"The root password can`t be less than {RootMinimumLenght} and more than {RootMaximumLenght} symbols."));

            if (!string.IsNullOrEmpty(download) &&
                !string.IsNullOrWhiteSpace(download) &&
                !(download.Length >= DownloadMinimumLenght && download.Length <= DownloadMaximumLenght))
                return Result<Descriptor>.Failure(error => error.InvalidArgument($"The download password can`t be less than {DownloadMinimumLenght} and more than {DownloadMaximumLenght} symbols."));

            if ((!string.IsNullOrEmpty(root) || !string.IsNullOrEmpty(download)) &&
                hashFactory == null)
                throw new ArgumentException("The hash factory can`t be null, if root or download password not null.");

            return Constructor(Guid.NewGuid(),
                               name,
                               path,
                               weight,
                               uploaded,
                               lifetime,
                               !string.IsNullOrEmpty(root) ? hashFactory.Invoke(root) : null,
                               !string.IsNullOrEmpty(download) ? hashFactory.Invoke(download) : null);
        }
    }
}
