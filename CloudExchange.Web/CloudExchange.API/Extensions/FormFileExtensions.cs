namespace CloudExchange.API.Extensions
{
    public static class FormFileExtensions
    {
        public static string GetName(this IFormFile file)
        {
            return file.FileName;
        }

        public static int GetWeight(this IFormFile file)
        {
            return (int)(file.Length / 1000);
        }

        public static Stream GetData(this IFormFile file)
        {
            return file.OpenReadStream();
        }
    }
}
