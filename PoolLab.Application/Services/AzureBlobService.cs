using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using PoolLab.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadFileImageAsync(string containerName, IFormFile file)
        {
            try
            {
                // Get a reference to a container
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                // Create the container if it does not exist.
                await containerClient.CreateIfNotExistsAsync();

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + ".jpg");

                // Open the file and upload its data
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // Return the URI of the blob (file)
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UploadQRCodeImageAsync(string containerName, byte[] fileBytes)
        {
            try
            {
                // Get a reference to a container
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                // Create the container if it does not exist.
                await containerClient.CreateIfNotExistsAsync();

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + ".jpg");

                // Open the file and upload its data
                using (var stream = new MemoryStream(fileBytes))
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // Return the URI of the blob (file)
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
