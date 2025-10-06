namespace CloudExchange.Application.Dto
{
    public class DescriptorDto
    {
        public DescriptorDto(Guid id,
                             string name,
                             long weight,
                             long createdAt,
                             long expiresAt,
                             int lifetime)
        {
            Id = id;
            Name = name;
            Weight = weight;
            CreatedAt = createdAt;
            Lifetime = lifetime;
        }

        public Guid Id { get; }

        public string Name { get; }

        public long Weight { get; }

        public long CreatedAt { get; }

        public int Lifetime { get; }
    }
}
