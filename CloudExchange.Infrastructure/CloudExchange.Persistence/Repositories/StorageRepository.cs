using Amazon.S3;
using Amazon.S3.Model;
using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Repositories;
using CloudExchange.Application.Dto;
using CloudExchange.Application.Extensions;
using CloudExchange.Domain.Aggregates;
using CloudExchange.Persistence.Abstractions.Clients;
using CloudExchange.Persistence.Options;
using Microsoft.Extensions.Options;
using System.Net;

namespace CloudExchange.Persistence.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly IAmazonS3 _internalS3Client;

        //разделение на internal и external нужно для генерации pre-sign ссылок для клиента
        //при использовании нормального хранилища S3 такое разделение будет лишним, а для эмулятора localstack в докере - это нужно
        private readonly IAmazonS3 _externalAmazonS3;

        private readonly ITimeProvider _timePovider;

        private readonly IStorageKeyProvider _storageKeyProvider;

        private readonly StorageOptions _storageOptions;

        public StorageRepository(IInternalAmazonS3 internalS3Client,
                                 IExternalAmazonS3 externalAmazonS3,
                                 ITimeProvider timePovider,
                                 IStorageKeyProvider storageKeyProvider,
                                 IOptions<StorageOptions> storageOptions)
        {
            _internalS3Client = internalS3Client.Client;
            _externalAmazonS3 = externalAmazonS3.Client;
            _timePovider = timePovider;
            _storageKeyProvider = storageKeyProvider;
            _storageOptions = storageOptions.Value;
        }

        public async Task<string?> DownloadAsync(DescriptorAggregate descriptor, CancellationToken cancellation = default)
        {
            try
            {
                await _internalS3Client.GetObjectMetadataAsync(_storageOptions.Bucket, _storageKeyProvider.Get(descriptor));
            }
            catch (AmazonS3Exception exception) when (exception.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                BucketName = _storageOptions.Bucket,
                Key = _storageKeyProvider.Get(descriptor),
                Expires = _timePovider.NowDateTime()
                                      .AddSeconds(_storageOptions.DownloadLifetime),
                Verb = HttpVerb.GET,
                Protocol = Protocol.HTTP,
            };

            return await _externalAmazonS3.GetPreSignedURLAsync(request);
        }

        public async Task<string> BeginUploadAsync(DescriptorAggregate descriptor, CancellationToken cancellation = default)
        {
            InitiateMultipartUploadRequest request = new InitiateMultipartUploadRequest
            {
                BucketName = _storageOptions.Bucket,
                Key = _storageKeyProvider.Get(descriptor),
                ContentType = "application/octet-stream"
            };

            InitiateMultipartUploadResponse response = await _internalS3Client.InitiateMultipartUploadAsync(request);

            return response.UploadId;
        }

        public async Task<string> ContinueUploadAsync(string key, string id, int part, CancellationToken cancellation = default)
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                BucketName = _storageOptions.Bucket,
                Key = key,
                Verb = HttpVerb.PUT,
                UploadId = id,
                PartNumber = part,
                Expires = _timePovider.NowDateTime()
                                      .AddSeconds(_storageOptions.UploadLifetime),
                Protocol = Protocol.HTTP,
            };

            return await _externalAmazonS3.GetPreSignedURLAsync(request);
        }

        public async Task CompleteUploadAsync(string key, string id, IReadOnlyCollection<PartDto> parts, CancellationToken cancellation = default)
        {
            CompleteMultipartUploadRequest request = new CompleteMultipartUploadRequest
            {
                BucketName = _storageOptions.Bucket,
                Key = key,
                UploadId = id
            };

            foreach (PartDto part in parts)
            {
                request.AddPartETags(new PartETag(part.Number, part.Tag));
            }

            await _internalS3Client.CompleteMultipartUploadAsync(request);
        }

        public async Task AbortUploadAsync(string key, string id, CancellationToken cancellation = default)
        {
            AbortMultipartUploadRequest request = new AbortMultipartUploadRequest
            {
                BucketName = _storageOptions.Bucket,
                Key = key,
                UploadId = id
            };

            await _internalS3Client.AbortMultipartUploadAsync(request);
        }

        public async Task DeleteAsync(string key, CancellationToken cancellation = default)
        {
            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = _storageOptions.Bucket,
                Key = key,
            };

            await _internalS3Client.DeleteObjectAsync(request);
        }
    }
}
