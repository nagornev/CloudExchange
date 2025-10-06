namespace CloudExchange.Application.Dto
{
    public class BeginUploadDto
    {
        public BeginUploadDto(string id, string key, Guid descriptorId)
        {
            Id = id;
            Key = key;
            DescriptorId = descriptorId;
        }

        public string Id { get; }

        public string Key { get; }

        public Guid DescriptorId { get; }
    }
}
