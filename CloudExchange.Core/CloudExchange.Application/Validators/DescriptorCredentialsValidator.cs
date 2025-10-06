using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Abstractions.Validators;
using System.Security.Cryptography;
using System.Text;

namespace CloudExchange.Application.Validators
{
    public class DescriptorCredentialsValidator : IDescriptorCredentialsValidator
    {
        private readonly IDescriptorCredentialsHashProvider _descriptorCredentialsHashProvider;

        public DescriptorCredentialsValidator(IDescriptorCredentialsHashProvider descriptorCredentialsHashProvider)
        {
            _descriptorCredentialsHashProvider = descriptorCredentialsHashProvider;
        }

        public bool Validate(string value, string salt, string hash)
        {
            return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(_descriptorCredentialsHashProvider.Hash(value, salt)),
                                                           Encoding.UTF8.GetBytes(hash));
        }
    }
}
