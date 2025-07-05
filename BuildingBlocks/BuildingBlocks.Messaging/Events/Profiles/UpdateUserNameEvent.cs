namespace BuildingBlocks.Messaging.Events.Profiles;

public record UpdateUserNameEvent : IntegrationEvent
{
    public Guid ProfileId { get; init; }
    public string ProfileName { get; init; } = string.Empty;
}