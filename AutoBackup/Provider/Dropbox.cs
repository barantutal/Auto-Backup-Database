using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoBackup.Provider
{
    public class Dropbox
    {
        public async Task Upload(string file_path, string file_name)
        {
            using var dbx = new DropboxClient(Program.dropboxToken);
            await ChunkUpload(dbx, file_path, file_name);
        }

        private async Task ChunkUpload(DropboxClient client, string file_path, string file_name)
        {
            const int chunkSize = 50 * 1024 * 1024;

            var fileContent = System.IO.File.ReadAllBytes(file_path);
            new Random().NextBytes(fileContent);

            using var stream = new MemoryStream(fileContent);
            var numChunks = (int)Math.Ceiling((double)stream.Length / chunkSize);

            byte[] buffer = new byte[chunkSize];
            string sessionId = null;

            for (var idx = 0; idx < numChunks; idx++)
            {
                var byteRead = stream.Read(buffer, 0, chunkSize);

                using MemoryStream memStream = new MemoryStream(buffer, 0, byteRead);
                if (idx == 0)
                {
                    var result = await client.Files.UploadSessionStartAsync(body: memStream);
                    sessionId = result.SessionId;
                }

                else
                {
                    UploadSessionCursor cursor = new UploadSessionCursor(sessionId, (ulong)(chunkSize * idx));

                    if (idx == numChunks - 1)
                    {
                        await client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(Program.dropboxFolder + file_name), memStream);
                    }

                    else
                    {
                        await client.Files.UploadSessionAppendV2Async(cursor, body: memStream);
                    }
                }
            }
        }
    }
}
