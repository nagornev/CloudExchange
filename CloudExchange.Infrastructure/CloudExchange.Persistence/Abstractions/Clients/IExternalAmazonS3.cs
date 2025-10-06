using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Persistence.Abstractions.Clients
{
    public interface IExternalAmazonS3
    {
        IAmazonS3 Client { get; }
    }
}
