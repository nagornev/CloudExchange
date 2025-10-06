using CloudExchange.Application.Dto;

namespace CloudExchange.API.Contracts
{
    public class CompleteUploadContract
    {
        public CompleteUploadContract(string id, string key, IReadOnlyCollection<PartDto> parts)
        {
            Id = id;
            Key = key;
            Parts = parts;
        }

        public string Id { get; }

        public string Key { get; }

        public IReadOnlyCollection<PartDto> Parts { get; }
    }
}
