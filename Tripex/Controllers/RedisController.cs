using Microsoft.AspNetCore.Mvc;
using Tripex.Core.Domain.Interfaces.Repositories;

namespace Tripex.Controllers;

public class RedisController(IRedisRepository repo) : BaseApiController
{
  [HttpGet("{key}")]
  public async Task<ActionResult> GetValue(string key)
  {
    var value = await repo.GetValue(key);
    return Ok(value ?? "No bro");
  }
}
