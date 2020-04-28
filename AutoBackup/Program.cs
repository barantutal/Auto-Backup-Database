using System;
using System.Threading.Tasks;
using AutoBackup.CronJobs.Quartz;

namespace AutoBackup
{
    class Program
    {
        public static string cronExpression = "0 0 */4 ? * *";
        public static string dropboxToken = "**YOUR DROPBOX APP TOKEN**";
        public static int delay = 10000;
        
        static async Task Main(string[] args)
        {
            await Initialize.Jobs();
            Console.ReadLine();
        }
    }
}
