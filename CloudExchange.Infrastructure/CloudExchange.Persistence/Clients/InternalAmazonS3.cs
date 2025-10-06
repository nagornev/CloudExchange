using Amazon.S3;
using CloudExchange.Persistence.Abstractions.Clients;
using CloudExchange.Persistence.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Persistence.Clients
{
    public class InternalAmazonS3 : IInternalAmazonS3
    {
        public IAmazonS3 Client { get; }

        public InternalAmazonS3(IOptions<AmazonS3Options> amazonS3Options)
        {
            Client = new AmazonS3Client(amazonS3Options.Value.AccessKey,
                                        amazonS3Options.Value.SecretKey,
                                        new AmazonS3Config()
                                        {
                                            ServiceURL = $"http://{amazonS3Options.Value.Host}:{amazonS3Options.Value.Port}",
                                            ForcePathStyle = true,
                                            UseHttp = true,
                                        });
        }
    }
}
