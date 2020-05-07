using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.IO;
using System.Threading.Tasks;

namespace AutoBackup.Provider
{
    public class GoogleCloudStorage
    {
        private readonly GoogleCredential googleCredential;
        private readonly StorageClient storageClient;
        private readonly string bucketName;

        public GoogleCloudStorage()
        {
            googleCredential = GoogleCredential.FromFile(Program.googleCredentialJsonFile);
            storageClient = StorageClient.Create(googleCredential);
            bucketName = Program.googleStorageBucketName;
        }

        public async Task<string> UploadFileAsync(string file_path, string file_name)
        {
            var fileContent = System.IO.File.ReadAllBytes(file_path);
            using var memoryStream = new MemoryStream(fileContent);
            var dataObject = await storageClient.UploadObjectAsync(bucketName, file_name, null, memoryStream);
            return dataObject.MediaLink;
        }

        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            await storageClient.DeleteObjectAsync(bucketName, fileNameForStorage);
        }
    }
}
