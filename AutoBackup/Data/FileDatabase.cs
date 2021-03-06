﻿using AutoBackup.Entities;
using AutoBackup.Infrastructure.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup.Database
{
    public class FileDatabase
    {
        readonly string filename = "database.txt";

        public void Add(string file_name)
        {
            var backupList = !File.Exists(filename) ? 
                new List<BackupFile>() :
                JsonConvert.DeserializeObject<List<BackupFile>>(File.ReadAllText(filename));

            backupList.Add(new BackupFile
            {
                BackupDate = DateTime.UtcNow,
                FileName = file_name
            });

            WriteDatabase(JsonConvert.SerializeObject(backupList));
        }

        public async Task RemoveOldFiles(IProvider provider)
        {
            var json = File.ReadAllText(filename);

            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            var backupList = JsonConvert.DeserializeObject<List<BackupFile>>(json);

            if(backupList.Count > Program.totalBackupFilesToKeep)
            {
                var filesToDelete = backupList
                    .OrderBy(p => p.BackupDate)
                    .Take(backupList.Count - Program.totalBackupFilesToKeep)
                    .ToList();

                foreach(var file in filesToDelete)
                {
                    await provider.DeleteAsync(file.FileName);
                }

                backupList = backupList.Except(filesToDelete).ToList();

                WriteDatabase(JsonConvert.SerializeObject(backupList));
            }
        }

        void WriteDatabase(string data)
        {
            using var fileStream = File.Create(filename);
            byte[] bytes = new UTF8Encoding(true).GetBytes(data);
            fileStream.Write(bytes, 0, bytes.Length);
        }
    }
}
