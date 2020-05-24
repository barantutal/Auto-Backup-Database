using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackup.Infrastructure.Provider
{
    public class GoogleCloudPlatformProvider : IProvider
    {
        const string googleCredentialJsonFile = "**storage.json";
        const string googleStorageBucketName = "**bucket-name**";
        readonly StorageClient storageClient;

        public GoogleCloudPlatformProvider()
        {
            var googleCredential = GoogleCredential.FromFile(googleCredentialJsonFile);
            storageClient = StorageClient.Create(googleCredential);
        }

        public async Task UploadAsync(string file_path, string file_name)
        {
            var fileContent = System.IO.File.ReadAllBytes(file_path);
            using var memoryStream = new MemoryStream(fileContent);
            await storageClient.UploadObjectAsync(googleStorageBucketName, file_name, null, memoryStream);
        }

        public async Task DeleteAsync(string fileNameForStorage)
        {
            await storageClient.DeleteObjectAsync(googleStorageBucketName, fileNameForStorage);
        }
    }
}
