namespace CloudExchange.Application.Abstractions.Providers
{
    public interface IDescriptorCredentialsHashProvider
    {
        /// <summary>
        /// Hashes <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string Hash(string? value, string salt);
    }
}
