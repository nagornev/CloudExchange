namespace CloudExchange.API.Contracts
{
    public class CreateContract
    {
        public IFormFile File { get; set; }

        public int Lifetime { get; set; }

        public string? Root { get; set; }

        public string? Download { get; set; }
    }
}
