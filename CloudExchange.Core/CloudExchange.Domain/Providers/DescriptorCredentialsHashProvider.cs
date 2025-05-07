using CloudExchange.Domain.Abstractions.Providers;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CloudExchange.Domain.Providers
{
    public class DescriptorCredentialsHashProvider : IDescriptorCredentialsHashProvider
    {

        private readonly DescriptorCredentialsHashProviderOptions _options;

        public DescriptorCredentialsHashProvider(IOptions<DescriptorCredentialsHashProviderOptions> options)
        {
            _options = options.Value;
        }

        public string Hash(string value)
        {
            string hash = string.Empty;

            using (HMACSHA512 hasher = new HMACSHA512(Encoding.UTF8.GetBytes(_options.Key)))
            {
                byte[] buffer = hasher.ComputeHash(Encoding.UTF8.GetBytes(value));

                hash = Convert.ToBase64String(buffer);
            }

            return hash;
        }

        public bool Verify(string value, string hash)
        {
            if (value == null)
                return false;

            return Hash(value) == hash;
        }
    }
}
