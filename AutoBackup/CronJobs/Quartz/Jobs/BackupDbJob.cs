using Quartz;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup.CronJobs.Quartz.Jobs
{
    public class BackupDbJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var folder_path = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backups"));

                if (Directory.Exists(folder_path))
                {
                    Directory.Delete(folder_path);
                }

                Directory.CreateDirectory(folder_path);

                var file_name = $"alldb{DateTime.Now:dd-MM-yyyy-HH-mm}.sql";
                var file_path = Path.Combine(folder_path, file_name);

                var stringBuilder = new StringBuilder();
                stringBuilder.Append("mysqldump -u root_ --default-character-set=utf8 --all-databases > ");
                stringBuilder.Append(file_path);

                new Bin().Bash(stringBuilder.ToString());

                await Task.Delay(Program.delay);

                await new Provider.Dropbox().Upload(file_path, file_name);
            }
            catch (Exception ex)
            {
                using FileStream fileStream = File.Create(DateTime.UtcNow.ToString("error") + ".txt");
                byte[] bytes = new UTF8Encoding(true).GetBytes(ex.ToString());
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
