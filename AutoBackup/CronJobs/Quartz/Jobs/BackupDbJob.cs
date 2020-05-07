using AutoBackup.Database;
using AutoBackup.Enum;
using AutoBackup.Provider;
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
                    Directory.Delete(folder_path, true);
                }

                Directory.CreateDirectory(folder_path);

                var file_name = $"alldb{DateTime.Now:dd-MM-yyyy-HH-mm}.sql.gz";
                var file_path = Path.Combine(folder_path, file_name);

                var command = new StringBuilder();
                command.Append(Program.command);
                command.Append(file_path);

                new Bin().Bash(command.ToString());

                switch (Program.provider)
                {
                    case DataProvider.GoogleCloudPlatformStorage:
                        await new GoogleCloudStorage().UploadFileAsync(file_path, file_name);
                        break;
                    case DataProvider.Dropbox:
                        await new Provider.Dropbox().Upload(file_path, file_name);
                        break;
                }

                var database = new FileDatabase();
                database.Add(file_name);
                await database.RemoveOldFiles();
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
