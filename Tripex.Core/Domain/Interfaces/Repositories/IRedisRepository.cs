namespace Tripex.Core.Domain.Interfaces.Repositories;

public interface IRedisRepository
{
  public Task<string?> GetValue(string key);
  public Task SetValue(string key, string value);
}
