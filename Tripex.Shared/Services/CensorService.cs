using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Tripex.Core.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;

namespace Tripex.Core.Services
{
    public class CensorService(HttpClient httpClient, IConfiguration configuration) : ICensorService
    {
        private const string PHOTO_PROMPT = "Please check the attached photo for censorship compliance and respond only 'Yes' if it is appropriate or 'No' if it is not.";
        public async Task<string> CheckImageAsync(IFormFile photoToCheck)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("API key is not configured");

            var compressedPhoto = CompressImage(photoToCheck);
            var imageBase64 = ConvertImageToBase64(compressedPhoto);

            var requestContent = new
            {
                model = "gpt-4o",
                messages = new[]
                {
                    new { role = "system", content = "You are an assistant that only answers Yes or No." },
                    new { role = "user", content = PHOTO_PROMPT }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestContent), System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Headers = { { "Authorization", $"Bearer {apiKey}" } },
                Content = jsonContent
            };

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"OpenAI API error: {response.StatusCode} - {response.ReasonPhrase}. Details: {errorContent}");
            }

            var result = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonDocument.Parse(result);

            if (jsonResponse.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var answer = choices[0].GetProperty("message").GetProperty("content").GetString();
                return answer?.Trim() ?? "No response received.";
            }

            throw new InvalidOperationException("Unexpected response format from OpenAI API");
        }

        public async Task<string> CheckTextAsync(string text)
        {
            var apiKey = configuration["OpenAI:ApiKey"];

            var requestContent = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "system", content = "You are a content moderation assistant. Respond only 'Yes' if the text contains inappropriate or censored content, or 'No' if it does not." },
                    new { role = "user", content = text }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Headers = { { "Authorization", $"Bearer {apiKey}" } },
                Content = jsonContent
            };

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"OpenAI API error: {response.StatusCode} - {response.ReasonPhrase}. Details: {errorDetails}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonDocument.Parse(responseContent);

            if (jsonResponse.RootElement.TryGetProperty("choices", out var choices) &&
                choices.GetArrayLength() > 0)
            {
                var content = choices[0].GetProperty("message").GetProperty("content").GetString();
                return content?.Trim() ?? "No response received.";
            }

            throw new InvalidOperationException("Unexpected response format from OpenAI API");
        }

        private string ConvertImageToBase64(IFormFile photo)
        {
            using var memoryStream = new MemoryStream();
            photo.CopyTo(memoryStream);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        private IFormFile CompressImage(IFormFile photo)
        {
            using var image = Image.FromStream(photo.OpenReadStream());
            var resizedImage = new Bitmap(image, new Size(256, 256)); // Зменшення до 256x256
            var memoryStream = new MemoryStream();
            resizedImage.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return new FormFile(memoryStream, 0, memoryStream.Length, photo.Name, photo.FileName)
            {
                Headers = photo.Headers,
                ContentType = "image/jpeg"
            };
        }
    }
}
