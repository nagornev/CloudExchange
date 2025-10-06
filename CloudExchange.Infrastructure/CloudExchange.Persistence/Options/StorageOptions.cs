namespace CloudExchange.Persistence.Options
{
    public class StorageOptions
    {
        public StorageOptions(string bucket,
                              int downloadLifetime,
                              int uploadLifetime)
        {
            Bucket = bucket;
            DownloadLifetime = downloadLifetime;
            UploadLifetime = uploadLifetime;
        }

        public string Bucket { get; }

        public int DownloadLifetime { get; }

        public int UploadLifetime { get; }
    }
}
