using Amazon.S3;
using Amazon;
using Microsoft.Extensions.Options;
using Kventin.Services.Config;

namespace Kventin.Services.Infrastructure.Tools
{
    public class YandexCloudFileStorageProvider(IOptions<YandexCloudFileStorageOptions> options) : S3FileStorageProvider
    {
        private readonly YandexCloudFileStorageOptions _options = options.Value;
        protected override string BucketName => _options.BucketName;

        protected override IAmazonS3 CreateClient()
        {
            return new AmazonS3Client(
                _options.KeyId,
                _options.SecretKey,
                new AmazonS3Config
                {
                    ServiceURL = _options.ServiceUrl,
                    ForcePathStyle = true,
                });
        }
    }
}
