namespace BuildingBlocks.Messaging.Events.Profiles;

public record UpdateUserName : IntegrationEvent
{
    public string UserName { get; set; } = string.Empty;
}