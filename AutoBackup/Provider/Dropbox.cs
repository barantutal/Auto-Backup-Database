using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Provider
{
    public class Dropbox
    {
        public async Task Upload(string file_path, string file_name)
        {
            using var dbx = new DropboxClient(Program.dropboxToken);
            var byteArray = System.IO.File.ReadAllBytes(file_path);
            using var mem = new MemoryStream(byteArray);
            await dbx.Files.UploadAsync(
                    "/backups/" + file_name,
                    WriteMode.Overwrite.Instance,
                    body: mem);
        }
    }
}
