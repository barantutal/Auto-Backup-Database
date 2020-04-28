using AutoBackup.CronJobs.Quartz.Jobs;
using Quartz;
using Quartz.Impl;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBackup.CronJobs.Quartz
{
    public static class Initialize
    {
        public static async Task Jobs()
        {
            var _scheduler = await new StdSchedulerFactory().GetScheduler(new CancellationToken());
            await _scheduler.Start(new CancellationToken());
            await _scheduler.ScheduleJob(
                JobBuilder.Create<BackupDbJob>()
                .WithIdentity("BackupDbJob").Build(),
                TriggerBuilder.Create().WithIdentity("BackupDbJobCron")
                .StartNow().WithCronSchedule(Program.cronExpression).Build(), new CancellationToken());
        }
    }
}
