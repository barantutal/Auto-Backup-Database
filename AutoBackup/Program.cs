using System;
using System.Threading.Tasks;
using AutoBackup.CronJobs.Quartz;
using AutoBackup.Enum;

namespace AutoBackup
{
    class Program
    {
        public const string cronExpression = "0 0 */2 ? * *"; // Cron job every 2 hours
        public const int totalBackupFilesToKeep = 50;

        //Provider selection
        public const DataProvider provider = DataProvider.GoogleCloudPlatformStorage;

        static async Task Main(string[] args)
        {
            await Jobs.Initialize();
            Console.ReadLine();
        }
    }
}
