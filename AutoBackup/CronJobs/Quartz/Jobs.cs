using Quartz;
using Quartz.Impl;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBackup.CronJobs.Quartz
{
    public static class Jobs
    {
        public static async Task Initialize()
        {
            var _scheduler = await new StdSchedulerFactory().GetScheduler(new CancellationToken());
            await _scheduler.Start(new CancellationToken());
            await _scheduler.ScheduleJob(
                    JobBuilder.Create<BackupDbJob>().WithIdentity("BackupDbJob").Build(),
                    TriggerBuilder.Create().WithIdentity("BackupDbJobCron")
                    .WithCronSchedule(Program.cronExpression).StartNow().Build(), new CancellationToken()
                );
        }
    }
}
