using System;
using System.Threading.Tasks;
using AutoBackup.CronJobs.Quartz;
using AutoBackup.Enum;

namespace AutoBackup
{
    class Program
    {
        public const string cronExpression = "0 0 */2 ? * *"; // Cron job every 2 hours

        //Dropbox Settings
        public const string dropboxToken = "**YOUR DROPBOX APP TOKEN**";
        public const string dropboxFolder = "/backups/";

        //Google Cloud Platform Settings
        public const string googleCredentialJsonFile = "**storage.json";
        public const string googleStorageBucketName = "**bucket-name**";

        //Provider selection
        public const DataProvider provider = DataProvider.GoogleCloudPlatformStorage;
        public const int totalBackupFilesToKeep = 50;

        static async Task Main(string[] args)
        {
            await Initialize.Jobs();
            Console.ReadLine();
        }
    }
}
