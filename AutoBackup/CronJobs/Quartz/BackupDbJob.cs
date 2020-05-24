using AutoBackup.Database;
using AutoBackup.Infrastructure.Provider;
using Quartz;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup.CronJobs.Quartz
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
                command.Append("mysqldump -u root_ ");
                command.Append("--routines ");
                command.Append("--default-character-set=utf8 ");
                command.Append("--all-databases ");
                command.Append("| gzip > ");
                command.Append(file_path);

                Bin.Bash(command.ToString());

                var provider = new ProviderFactory().GetProvider(Program.provider);
                await provider.UploadAsync(file_path, file_name);

                var database = new FileDatabase();
                database.Add(file_name);
                await database.RemoveOldFiles(provider);
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
