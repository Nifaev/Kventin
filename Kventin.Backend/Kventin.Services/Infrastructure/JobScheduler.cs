using Kventin.Services.Config;
using Kventin.Services.Infrastructure.Jobs;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;

namespace Kventin.Services.Infrastructure
{
    public class JobScheduler
    {
        public static async Task Start(string connectionString, IOptions<LessonsGeneratorOptions> options)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<CustomScheduleJob>()
                .WithIdentity("CustomScheduleJob")
                .UsingJobData("connectionString", connectionString)
                .UsingJobData("weeksCount", options.Value.WeeksCount)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("CustomScheduleTrigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                   .WithIntervalInMinutes(30)  // Интервал в 30 минут
                   .RepeatForever())
                //.WithSchedule(CronScheduleBuilder.CronSchedule("0 0 20 ? * SUN *")) // 20:00 Sunday
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
