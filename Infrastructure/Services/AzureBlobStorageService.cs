using Application.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task DeleteFileAsync(string relativePath, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return;
            }
            var parts = relativePath.Split(new[] { '/' }, 2);
            var containerName = parts[0];
            var blobName = parts[1];
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public string GetFileUrl(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return string.Empty;
            }

            try
            {
                var parts = relativePath.Split(new[] { '/' }, 2);
                var containerName = parts[0];
                var blobName = parts[1];

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = containerName,
                    BlobName = blobName,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                var sasUri = blobClient.GenerateSasUri(sasBuilder);
                return sasUri.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public async Task<string> SaveFileAsync(IFormFile file, string subFolder, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }

            var containerName = subFolder.ToLower().Replace(" ", "-");
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.None, cancellationToken: cancellationToken);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            var bobClient = containerClient.GetBlobClient(uniqueFileName);
            await bobClient.UploadAsync(file.OpenReadStream(), cancellationToken);
            return $"{containerName}/{uniqueFileName}";
        }

        public async Task<List<string>> SaveFileAsync(List<IFormFile> files, string subFolder, CancellationToken cancellationToken)
        {
            if (files == null || files.Count == 0)
            {
                return new List<string>();
            }

            var uploadTasks = new List<Task<string>>();
            foreach (var file in files)
            {
                uploadTasks.Add(SaveFileAsync(file, subFolder, cancellationToken));
            }

            var urls = await Task.WhenAll(uploadTasks);
            return urls.ToList();
        }
    }
}
