using Kventin.DataAccess.Domain;

namespace Kventin.Services.Infrastructure.Extensions
{
    public static class FileRecordExtensions
    {
        private const double BYTES_INT_KILOBYTE = 1024;

        public static string SizeToString(this FileRecord fileRecord)
        {
            var bytesCount = fileRecord.FileSize;
            var kiloBytesCount = bytesCount / BYTES_INT_KILOBYTE;
            var megaBytesCount = kiloBytesCount / BYTES_INT_KILOBYTE;
            var gigaBytesCount = megaBytesCount / BYTES_INT_KILOBYTE;

            if ((long)gigaBytesCount > 0)
                return $"{gigaBytesCount.ToString("F2")} ГБ";

            if ((long)megaBytesCount > 0)
                return $"{megaBytesCount.ToString("F2")} МБ";

            if ((long)kiloBytesCount > 0)
                return $"{kiloBytesCount.ToString("F2")} КБ";

            return $"{bytesCount.ToString("F2")} Байт";
        }
    }
}
