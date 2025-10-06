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
    public class ExternalAmazonS3 : IExternalAmazonS3
    {
        public IAmazonS3 Client { get; }

        public ExternalAmazonS3(IOptions<AmazonS3Options> amazonS3Options)
        {
            Client = new AmazonS3Client(amazonS3Options.Value.AccessKey,
                                        amazonS3Options.Value.SecretKey,
                                        new AmazonS3Config()
                                        {
                                            ServiceURL = $"http://{amazonS3Options.Value.Ehost}:{amazonS3Options.Value.Eport}",
                                            ForcePathStyle = true,
                                            UseHttp = true,
                                        });
        }
    }
}
