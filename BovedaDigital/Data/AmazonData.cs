using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Mvc;
using SmcApi.Models;
using System.IO;
using System.Net;
using System.Text.Json;

namespace SmcApi.Data
{
    public class AmazonData
    {

        public static async Task<string> UploadFileAsync(IFormFile file, string bucket, string credenciales, int version)
        {
            try
            {
                AwsCredenciales credentials = JsonSerializer.Deserialize<AwsCredenciales>(credenciales);

                using (var amazonS3client = new AmazonS3Client(credentials.AccessKey,
                   credentials.SecretKey, RegionEndpoint.GetBySystemName(credentials.Region)))
                {
                    using (var newMemoryStream = new MemoryStream())
                    {
                        file.CopyTo(newMemoryStream);

                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = newMemoryStream,
                            Key = file.FileName + ".V" + version,
                            BucketName = bucket,
                            ContentType = file.ContentType,

                        };

                        var fileTransferUtility = new TransferUtility(amazonS3client);

                        await fileTransferUtility.UploadAsync(uploadRequest);

                        string mediaLink = $"https://{bucket}.s3.{credentials.Region}.amazonaws.com/{file.FileName + ".V" + version}";


                        return mediaLink;
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<byte[]> DownloadFileAsync(string fileName, string bucket, string credenciales, int version)
        {
            try
            {
                AwsCredenciales credentials = JsonSerializer.Deserialize<AwsCredenciales>(credenciales);

                using (var amazonS3client = new AmazonS3Client(credentials.AccessKey,
                   credentials.SecretKey, RegionEndpoint.GetBySystemName(credentials.Region)))
                {
                    using (var newMemoryStream = new MemoryStream())
                    {
                        using (var response = await amazonS3client.GetObjectAsync(bucket, fileName + ".V" + version))
                        {
                            if(response.HttpStatusCode == HttpStatusCode.OK)
                            {
                                await response.ResponseStream.CopyToAsync(newMemoryStream);
                                return newMemoryStream.ToArray();
                            }
                            else
                            {
                                throw new Exception("Error");
                            }
                        }
                    }
                    
                }
            }
            catch(Exception)
            {
                throw;
            }
        }


    }
}
