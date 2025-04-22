namespace BuildingBlocks.Messaging.Events.Cache;

public record CacheBasicInfoEvent : IntegrationEvent
{
    public Guid ProfileId { get; init; }
}