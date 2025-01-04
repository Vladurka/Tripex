using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class S3Controller(IS3FileService s3FileService) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string fileName)
        {
            await s3FileService.UploadFileAsync(file, fileName);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> ListFiles()
        {
            var files = await s3FileService.ListFilesAsync();

            return Ok(files);
        }

        [HttpGet("download/{fileName}")]
        public async Task<ActionResult> DownloadFile(string fileName)
        {
            await s3FileService.DownloadFileAsync(fileName);
            return Ok();
        }
    }
}
