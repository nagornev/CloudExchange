using Org.BouncyCastle.Pkcs;

namespace CloudExchange.Database.Entities
{
    public class DescriptorEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public long Weight { get; set; }

        public long Uploaded { get; set; }

        public int Lifetime { get; set; }

        public string? Root { get; set; }

        public string? Download { get; set; }
    }
}
