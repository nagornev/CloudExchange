namespace CloudExchange.API.Contracts
{
    public class ContinueUploadContract
    {
        public ContinueUploadContract(string id, string key, int part)
        {
            Id = id;
            Key = key;
            Part = part;
        }

        public string Id { get; }

        public string Key { get; }

        public int Part { get; }
    }
}
