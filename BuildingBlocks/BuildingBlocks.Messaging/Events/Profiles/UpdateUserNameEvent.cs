namespace BuildingBlocks.Messaging.Events.Profiles;

public record UpdateUserNameEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string ProfileName { get; set; } = string.Empty;
}