namespace CloudExchange.API.Contracts
{
    public class BeginUploadContract
    {
        public BeginUploadContract(string name, long weight, int lifetime, string? root, string? download)
        {
            Name = name;
            Weight = weight;
            Lifetime = lifetime;
            Root = root;
            Download = download;
        }

        public string Name { get; }
        public long Weight { get; }
        public int Lifetime { get; }
        public string? Root { get; }
        public string? Download { get; }
    }
}
