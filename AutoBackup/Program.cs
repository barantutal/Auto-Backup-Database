using System;
using System.Threading.Tasks;
using AutoBackup.CronJobs.Quartz;

namespace AutoBackup
{
    class Program
    {
        public static string cronExpression = "0 0 */4 ? * *"; // Every 4 hours
        public static string dropboxToken = "**YOUR DROPBOX APP TOKEN**";
        public static string dropboxFolder = "/backups/";
        public static string command = "mysqldump -u root_ --default-character-set=utf8 --all-databases > ";
        public static int totalBackupFilesToKeep = 6;

        static async Task Main(string[] args)
        {
            await Initialize.Jobs();
            Console.ReadLine();
        }
    }
}
