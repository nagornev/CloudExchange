namespace CloudExchange.Application.Options
{
    public class DescriptorOptions
    {
        public DescriptorOptions(string deletionInterval)
        {
            DeletionInterval = deletionInterval;
        }

        public string DeletionInterval { get; }
    }
}
