namespace CloudExchange.Application.Dto
{
    public class DownloadDto
    {
        public DownloadDto(DescriptorDto descriptor, string url)
        {
            Descriptor = descriptor;
            Url = url;
        }

        public DescriptorDto Descriptor { get; }

        public string Url { get; }
    }
}
