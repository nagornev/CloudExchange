using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Persistence.Options
{
    public class AmazonS3Options
    {
        public AmazonS3Options(string host,
                               ushort port,
                               string ehost,
                               ushort eport,
                               string scope,
                               string accessKey,
                               string secretKey,
                               string region)
        {
            Host = host;
            Port = port;
            Ehost = ehost;
            Eport = eport;
            Scope = scope;
            AccessKey = accessKey;
            SecretKey = secretKey;
            Region = region;
        }

        public string Host { get; }

        public ushort Port { get; }

        public string Ehost { get; }

        public ushort Eport { get; }

        public string Scope { get; }

        public string AccessKey { get; }

        public string SecretKey { get; }

        public string Region { get; }
    }
}
