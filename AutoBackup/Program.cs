using System;
using System.Threading.Tasks;
using AutoBackup.CronJobs.Quartz;
using AutoBackup.Enum;
using AutoBackup.Provider;

namespace AutoBackup
{
    class Program
    {
        public static string cronExpression = "0 0 */4 ? * *"; // Cron job every 4 hours
        
        //Dropbox Settings
        public static string dropboxToken = "**YOUR DROPBOX APP TOKEN**";
        public static string dropboxFolder = "/backups/";

        //GCP Settings
        public static string googleCredentialJsonFile = "storage.json";
        public static string googleStorageBucketName = "bucketName";

        public static DataProvider provider = DataProvider.GoogleCloudPlatformStorage;
        public static string command = "mysqldump -u root_ --default-character-set=utf8 --all-databases | gzip -c > ";
        public static int totalBackupFilesToKeep = 12;

        static async Task Main(string[] args)
        {
            await Initialize.Jobs();
            Console.ReadLine();
        }
    }
}
