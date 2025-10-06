namespace CloudExchange.Application.Dto
{
    public class PartDto
    {
        public PartDto(int number, string tag)
        {
            Number = number;
            Tag = tag;
        }

        public int Number { get; }

        public string Tag { get; }
    }
}
