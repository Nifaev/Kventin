namespace Kventin.Services.Config
{
    public class YandexCloudFileStorageOptions
    {
        public required string KeyId { get; set; }
        public required string SecretKey { get; set; }
        public required string BucketName { get; set; }
        public required string Region { get; set; }
        public required string ServiceUrl { get; set; }
    }
}
