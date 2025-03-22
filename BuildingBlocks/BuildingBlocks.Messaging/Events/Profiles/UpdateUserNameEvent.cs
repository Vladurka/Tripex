namespace BuildingBlocks.Messaging.Events.Profiles;

public record UpdateUserNameEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}