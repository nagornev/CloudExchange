namespace MessageContracts
{
    public record DescriptorDeletedMessageContract(Guid DescriptorId, string Name, string Upload)
    {
    }
}
