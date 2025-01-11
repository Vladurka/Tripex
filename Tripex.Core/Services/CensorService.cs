using System.Text.Json;
using Tripex.Core.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Tripex.Core.Services
{
    public class CensorService(HttpClient httpClient, IConfiguration configuration) : ICensorService
    {
        private const string TEXT_PROMPT = "You are a content moderation assistant. Respond only 'Yes' if the text contains inappropriate or censored content, or 'No' if it does not.";

        public async Task<string> CheckTextAsync(string text)
        {
            var apiKey = configuration["OpenAI:ApiKey"];

            var requestContent = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "system", content = TEXT_PROMPT },
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
    }
}
