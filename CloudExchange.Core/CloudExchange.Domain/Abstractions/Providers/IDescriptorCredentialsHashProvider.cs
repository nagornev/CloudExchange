namespace CloudExchange.Domain.Abstractions.Providers
{
    public interface IDescriptorCredentialsHashProvider
    {
        string Hash(string value);

        bool Verify(string value, string hash);
    }
}
