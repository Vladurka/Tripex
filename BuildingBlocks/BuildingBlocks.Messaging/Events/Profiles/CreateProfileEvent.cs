namespace BuildingBlocks.Messaging.Events.Profiles;

public record CreateProfileEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
}