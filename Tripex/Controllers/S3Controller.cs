using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;

namespace Tripex.Controllers
{
    public class S3Controller(IAmazonS3 s3Client) : BaseApiController
    {
        private const string BUCKET_NAME = "tripex";

        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing or empty.");

            using var stream = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = BUCKET_NAME,
                Key = file.FileName,
                InputStream = stream,
                ContentType = file.ContentType
            };

            await s3Client.PutObjectAsync(request);
            return Ok($"File '{file.FileName}' uploaded successfully to bucket '{BUCKET_NAME}'.");
        }

        [HttpGet]
        public async Task<ActionResult> ListFiles()
        {
            var request = new ListObjectsV2Request
            {
                BucketName = BUCKET_NAME
            };

            var response = await s3Client.ListObjectsV2Async(request);
            var files = response.S3Objects.Select(o => o.Key).ToList();

            return Ok(files);

        }

        [HttpGet("download/{fileName}")]
        public async Task<ActionResult> DownloadFile(string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = BUCKET_NAME,
                Key = fileName
            };

            var response = await s3Client.GetObjectAsync(request);
            return File(response.ResponseStream, response.Headers["Content-Type"], fileName);
        }
    }
}
