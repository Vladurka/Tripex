namespace BuildingBlocks.Messaging.Events.Profiles;

public record CreateProfileEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
} 