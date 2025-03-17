using Kventin.Services.Infrastructure.Tools;
using Quartz;

namespace Kventin.Services.Infrastructure.Jobs
{
    public class CustomScheduleJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Задача \"CustomScheduleJob\" запущена в {DateTime.Now}");

            var connectionString = context.JobDetail.JobDataMap.GetString("connectionString") ?? string.Empty;

            var lessonGenerator = new LessonGenerator(connectionString);

            await lessonGenerator.GenerateLessons();

            Console.WriteLine($"Задача \"CustomScheduleJob\" успешно завершила работу в {DateTime.Now}");
        }
    }
}
