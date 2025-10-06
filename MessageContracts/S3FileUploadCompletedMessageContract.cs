namespace MessageContracts
{
    public record S3FileUploadCompletedMessageContract(IReadOnlyCollection<S3Record> Records);

    public record S3Record(S3Entity S3);

    public record S3Entity(S3Bucket Bucket, S3Object Object);

    public record S3Bucket(string Name);

    public record S3Object(string Key, long Size);

}
