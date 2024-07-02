using System;
using System.Threading.Tasks;
using Dropbox.Api;
using System.IO;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Mvc;
using SmcApi.Models;
using System.Text.Json;

namespace SmcApi.Data
{
    public class DropboxData
    {


        public static async Task<string> UploadAsync(IFormFile file, string bucket, string credenciales, int version)
        {
            try
            {
                AzureDropBox credentials = JsonSerializer.Deserialize<AzureDropBox>(credenciales);

                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);
                    newMemoryStream.Position = 0;
                    using (var dbx  = new DropboxClient(credentials.token))
                    {
                        var uploadFile = await dbx.Files.UploadAsync("/Public/" + file.FileName + ".V" + version, WriteMode.Overwrite.Instance, body: newMemoryStream);

                        var sharedLink = await dbx.Sharing.CreateSharedLinkWithSettingsAsync(uploadFile.PathLower);

                        
                        return sharedLink.Url;
                    }

                }

                
            }

            catch (Exception )
            {
                throw;
            }
        }

        public static async Task<byte[]> DownloadAsync(string filename, string bucket, string credenciales, int version)
        {
            try
            {
                AzureDropBox credentials = JsonSerializer.Deserialize<AzureDropBox>(credenciales);

                var dbx = new DropboxClient(credentials.token);

                MemoryStream ms = new MemoryStream();

                var resposnse = await dbx.Files.DownloadAsync("/Public/" + filename + ".V" + version);

                (await resposnse.GetContentAsStreamAsync()).CopyTo(ms);

                return ms.ToArray();
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
