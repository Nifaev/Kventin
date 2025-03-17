using Kventin.Services.Infrastructure.Jobs;
using Quartz;
using Quartz.Impl;

namespace Kventin.Services.Infrastructure
{
    public class JobScheduler
    {
        public static async Task Start(string connectionString)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<CustomScheduleJob>()
                .WithIdentity("CustomScheduleJob")
                .UsingJobData("connectionString", connectionString)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("CustomScheduleTrigger")
                //.StartNow()
                .WithSimpleSchedule(x => x
                   .WithIntervalInMinutes(30)  // Интервал в 30 минут
                   .RepeatForever())
                //.WithSchedule(CronScheduleBuilder.CronSchedule("0 0 20 ? * SUN *")) // 20:00 Sunday
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
