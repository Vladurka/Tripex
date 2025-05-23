namespace BuildingBlocks.Messaging.Events.Profiles;

public record IncrementPostCountEvent : IntegrationEvent
{
    public Guid ProfileId { get; init; }
}