using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SmcApi.Data
{
    public class CloudData
    {

        public static async Task<string> UploadFile(IFormFile fileUpload, string bucket, string credenciales, int version)
        {
            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, credenciales);

                GoogleCredential credential = null;
                using (var jsonStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    credential = GoogleCredential.FromStream(jsonStream);
                }

                
                using (var memoryStream = new MemoryStream())
                {
                    await fileUpload.CopyToAsync(memoryStream);

                    using (var storageClient = StorageClient.Create(credential))
                    {
                        var uploadFile = await storageClient.UploadObjectAsync(bucket, fileUpload.FileName + ".V" + version, fileUpload.ContentType, memoryStream);
                        
                        var url = uploadFile.MediaLink;
      

                        return url;
                    }
                }
            }

            catch (Exception ex)
            {
                
                throw;
            }

            finally
            {
                File.Delete(tempFile);
            }
        }



        
        public static async Task<byte[]> GetFile(string fileName, string bucket, string credenciales, int version)
        {
            string tempFile = Path.GetTempFileName();
            try
            {
                

                File.WriteAllText(tempFile, credenciales);
                GoogleCredential credential = null;
                using (var jsonStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    credential = GoogleCredential.FromStream(jsonStream);
                }

                using (var memoryStream = new MemoryStream())
                {
                    using (var storageClient = StorageClient.Create(credential))
                    {
                        var FileGet = await storageClient.DownloadObjectAsync(bucket, fileName + ".V" + version, memoryStream);
                        var newMemoryStream = new MemoryStream(memoryStream.ToArray());
                        newMemoryStream.Position = 0;
                        
                       
                        return newMemoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
            finally
            {
                File.Delete(tempFile);
            }
        }




    }
}

