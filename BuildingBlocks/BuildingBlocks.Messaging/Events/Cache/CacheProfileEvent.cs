namespace BuildingBlocks.Messaging.Events.Cache;

public record CacheProfileEvent : IntegrationEvent
{
    public Guid ProfileId { get; set; }
}