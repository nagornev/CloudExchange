namespace CloudExchange.Application.Dto
{
    public class DescriptorDto
    {
        public DescriptorDto(Guid id,
                             string name,
                             long weight,
                             long uploaded,
                             int lifetime)
        {
            Id = id;
            Name = name;
            Weight = weight;
            Uploaded = uploaded;
            Lifetime = lifetime;
        }

        public Guid Id { get; }

        public string Name { get; }

        public long Weight { get; }

        public long Uploaded { get; }

        public int Lifetime { get; }
    }
}
