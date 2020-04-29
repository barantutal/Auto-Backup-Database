using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBackup.Entities
{
    public class BackupFile
    {
        public string FileName { get; set; }
        public DateTime BackupDate { get; set; }
    }
}
