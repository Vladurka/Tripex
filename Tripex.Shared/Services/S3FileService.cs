using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class S3FileService(IAmazonS3 s3Client) : IS3FileService
    {
        private const string BUCKET_NAME = "tripex";
        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is missing or empty");

            using var stream = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = BUCKET_NAME,
                Key = fileName.ToString(),
                InputStream = stream,
                ContentType = file.ContentType
            };

            await s3Client.PutObjectAsync(request);

            string photoUrl = GetPreSignedURL(fileName, 10);

            return photoUrl;
        }

        public string GetPreSignedURL(string fileName, int hours)
        {
            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = BUCKET_NAME,
                Key = fileName,
                Expires = DateTime.UtcNow.AddHours(hours)
            };

            string temporaryUrl = s3Client.GetPreSignedURL(urlRequest);

            return temporaryUrl;
        }

        public async Task<List<string>> ListFilesAsync()
        {
            var request = new ListObjectsV2Request
            {
                BucketName = BUCKET_NAME
            };

            var response = await s3Client.ListObjectsV2Async(request);
            return response.S3Objects.Select(o => o.Key).ToList();
        }

        public async Task<(Stream FileStream, string ContentType, string FileName)> DownloadFileAsync(string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = BUCKET_NAME,
                Key = fileName
            };

            var response = await s3Client.GetObjectAsync(request);
            return (response.ResponseStream, response.Headers["Content-Type"], fileName);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty");

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = BUCKET_NAME,
                Key = fileName
            };

            await s3Client.DeleteObjectAsync(deleteRequest);
        }
    }
}
