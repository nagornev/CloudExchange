namespace CloudExchange.Application.Dto
{
    public class ContinueUploadDto
    {
        public ContinueUploadDto(string url)
        {
            Url = url;
        }

        public string Url { get; }
    }
}
