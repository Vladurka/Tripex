namespace BuildingBlocks.Auth;

public interface IJwtHelper
{
    public Guid GetUserIdByToken();
}