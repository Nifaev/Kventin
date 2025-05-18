using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;
using Kventin.Services.Interfaces.Tools;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Infrastructure.Tools
{
    public class FileRecordFactory : IFileRecordFactory
    {
        public FileRecord Create<T>(IFormFile file, string originalFileName, User uploadedByUser, FileLinkType fileLinkType, T? linkedEntity) where T : BaseEntity
        {
            var fileRecord =  new FileRecord
            {
                OriginalFileName = originalFileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                UploadedByUser = uploadedByUser,
                UploadedByUserId = uploadedByUser.Id,
                StorageFileName = $"{Guid.NewGuid()}_{originalFileName}",
                LinkedWith = fileLinkType,
            };

            if (fileLinkType != FileLinkType.None)
            {
                if (linkedEntity == null)
                    throw new NullReferenceException("Переданная для связи сущность не должна быть null");

                var propertyName = fileLinkType.ToString();

                // Получаем PropertyInfo нужного свойства
                var prop = typeof(FileRecord).GetProperty(propertyName);

                if (prop == null)
                    throw new InvalidOperationException($"Свойство '{propertyName}' не найден в сущности FileRecord");

                if (!prop.PropertyType.IsAssignableFrom(typeof(T)))
                    throw new InvalidOperationException($"Свойству '{propertyName}' нельзя присовить значение {typeof(T).Name}");

                // Присваиваем linkedEntity в свойство
                prop.SetValue(fileRecord, linkedEntity);
            }

            return fileRecord;
        }
    }
}
