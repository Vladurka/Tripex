using Azure.AI.ContentSafety;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.ContentModeration;

public class ContentModerationService(ContentSafetyClient client) : IContentModerationService
{
    public async Task<bool> ModeratePhoto(IFormFile photo)
    {
        if (photo == null || photo.Length == 0)
            throw new ArgumentException("Photo is required and cannot be empty.");
        
        using var memoryStream = new MemoryStream();
        await photo.OpenReadStream().CopyToAsync(memoryStream);
        var imageData = new ContentSafetyImageData(BinaryData.FromBytes(memoryStream.ToArray()));
        
        var request = new AnalyzeImageOptions(imageData);
        var response = await client.AnalyzeImageAsync(request);

        var categories = response.Value.CategoriesAnalysis;
        
        int? GetRawSeverity(ImageCategory category) =>
            categories.FirstOrDefault(c => c.Category == category)?.Severity;
        
        int NormalizeSeverity(int? severity)
        {
            if (severity is null || severity <= 1) return 0;
            if (severity <= 3) return 2;
            if (severity <= 5) return 4;
            return 6;
        }

        bool IsSevere(int value) => value != 0 && value >= 4;
        
        var hate = NormalizeSeverity(GetRawSeverity(ImageCategory.Hate));
        var selfHarm = NormalizeSeverity(GetRawSeverity(ImageCategory.SelfHarm));
        var sexual = NormalizeSeverity(GetRawSeverity(ImageCategory.Sexual));
        var violence = NormalizeSeverity(GetRawSeverity(ImageCategory.Violence));
        
        return !(IsSevere(hate) || IsSevere(selfHarm) || IsSevere(sexual) || IsSevere(violence));
    }
}