namespace CloudExchange.Application.Abstractions.Validators
{
    public interface IDescriptorCredentialsValidator
    {
        bool Validate(string value, string salt, string hash);
    }
}
