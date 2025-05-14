using Amazon.S3;
using Amazon.S3.Model;
using Kventin.Services.Interfaces.Tools;
using System.Net;

namespace Kventin.Services.Infrastructure.Tools
{
    public abstract class S3FileStorageProvider : IFileStorageProvider
    {
        protected abstract IAmazonS3 CreateClient();
        protected abstract string BucketName { get; }

        public async Task DeleteFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Некорректное имя файла", nameof(fileName));

            using var client = CreateClient();

            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = BucketName,
                    Key = fileName
                };

                var response = await client.DeleteObjectAsync(request);

                if (response.HttpStatusCode != HttpStatusCode.NoContent &&
                    response.HttpStatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Не удалось удалить файл. Status code: {response.HttpStatusCode}");
                }
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"S3-ошибка при удалении файла: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Неожиданная ошибка при удалении файла", ex);
            }
        }

        public async Task<MemoryStream> DownloadFileAsync(string storageFileName)
        {
            using var client = CreateClient();

            try
            {
                var response = await client.GetObjectAsync(BucketName, storageFileName);

                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw new Exception($"Не удалось скачать файл. Status code: {response.HttpStatusCode}");

                var memoryStream = new MemoryStream();

                await response.ResponseStream.CopyToAsync(memoryStream);

                memoryStream.Position = 0;

                return memoryStream;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FileNotFoundException("Файл не найден в облачном хранилище", storageFileName);
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"S3-ошибка при загрузке: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Неожиданная ошибка при скачивании файла", ex);
            }
        }

        public async Task UploadFileAsync(Stream stream, string storageFileName, string contentType)
        {
            using var client = CreateClient();

            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = BucketName,
                    Key = storageFileName,
                    InputStream = stream,
                    ContentType = contentType ?? "application/octet-stream",
                    AutoCloseStream = true
                };

                var response = await client.PutObjectAsync(request);

                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw new Exception($"Не удалось загрузить файл. Status code: {response.HttpStatusCode}");
            }
            catch (AmazonS3Exception ex)
            {
                // Обработка ошибок от Yandex S3
                throw new Exception($"S3-ошибка при загрузке файла: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Общая ошибка
                throw new Exception("Неожиданная ошибка при загрузке файла", ex);
            }
        }
    }
}
