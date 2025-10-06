using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace CloudExchange.Security.Providers
{
    public class DescriptorCredentialsHashProvider : IDescriptorCredentialsHashProvider
    {
        private readonly DescriptorCredentialsHashOptions _hashOptions;


        public DescriptorCredentialsHashProvider(IOptions<DescriptorCredentialsHashOptions> hashOptions)
        {
            _hashOptions = hashOptions.Value;
        }

        public string Hash(string? value, string salt)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(value),
                                                                      Convert.FromBase64String(salt),
                                                                      _hashOptions.Iterations,
                                                                      HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(_hashOptions.Size);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
