namespace CloudExchange.Application.Options
{
    public class DescriptorCredentialsHashOptions
    {
        public DescriptorCredentialsHashOptions(int size,
                                                int iterations)
        {
            Size = size;
            Iterations = iterations;
        }

        public int Size { get; }

        public int Iterations { get; }
    }
}
