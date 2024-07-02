using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using SmcApi.Models;
using System.Text.Json;

namespace SmcApi.Data
{
    public class AzureData
    {


        public static async Task<string> UploadAsync(IFormFile file, string bucket, string credenciales, int version)
        {
            try
            {
                AzureDropBox credentials = JsonSerializer.Deserialize<AzureDropBox>(credenciales);

                BlobContainerClient blobContainerClient = new BlobContainerClient(credentials.token, bucket);

                await blobContainerClient.CreateIfNotExistsAsync();

                BlobContentInfo blobContent = null;


                using(var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);
                    newMemoryStream.Position = 0;
                    blobContent = await blobContainerClient.UploadBlobAsync(file.FileName + ".V" + version, newMemoryStream);
                }

                string mediaLink = $"{blobContainerClient.Uri}/{file.FileName}.V{version}";


                return mediaLink;
            }
            
            catch(Exception) 
            {
                throw;
            }  

            
        }

        public static async Task<byte[]> DownloadAsync(string filename, string bucket, string credenciales, int version)
        {
            try
            {
                AzureDropBox credentials = JsonSerializer.Deserialize<AzureDropBox>(credenciales);

                BlobContainerClient blobContainerClient = new BlobContainerClient(credentials.token, bucket);

                await blobContainerClient.CreateIfNotExistsAsync();

                MemoryStream ms = new MemoryStream();

                await blobContainerClient.GetBlobClient(filename + ".V" + version).DownloadToAsync(ms);

                return ms.ToArray();
            }
            
            catch (Exception)
            {
                throw;
            }
        }
    }
}
