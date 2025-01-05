using StackExchange.Redis;
using Tripex.Core.Domain.Interfaces.Repositories;

namespace Tripex.Infrastructure.Persistence.Repositories;

public class RedisRepository(IConnectionMultiplexer redis) : IRedisRepository
{
  public async Task<string?> GetValue(string key)
  {
    var db = redis.GetDatabase();
    var value = await db.StringGetAsync(key);

    return value;
  }

  public async Task SetValue(string key, string value)
  {
    var db = redis.GetDatabase();
    await db.StringSetAsync(key, value);
  }
}
