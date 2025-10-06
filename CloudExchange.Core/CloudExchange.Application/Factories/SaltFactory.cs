using CloudExchange.Application.Abstractions.Factories;
using CloudExchange.Application.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace CloudExchange.Application.Factories
{
    public class SaltFactory : ISaltFactory
    {
        private readonly SaltOptions _saltOptions;

        public SaltFactory(IOptions<SaltOptions> saltOptions)
        {
            _saltOptions = saltOptions.Value;
        }

        public string Create()
        {
            byte[] salt = new byte[_saltOptions.Size];

            RandomNumberGenerator.Fill(salt);

            return Convert.ToBase64String(salt);
        }
    }
}
