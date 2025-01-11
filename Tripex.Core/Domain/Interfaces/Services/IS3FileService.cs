using Microsoft.AspNetCore.Http;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IS3FileService
    {
        public Task<string> UploadFileAsync(IFormFile file, string fileName);
        public string GetPreSignedURL(string fileName, int hours);
        public Task<List<string>> ListFilesAsync();
        public Task<(Stream FileStream, string ContentType, string FileName)> DownloadFileAsync(string fileName);
        public Task DeleteFileAsync(string fileName);
    }
}
