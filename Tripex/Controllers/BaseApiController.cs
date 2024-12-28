using Microsoft.AspNetCore.Mvc;
using Tripex.Core.Enums;

namespace Tripex.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult CheckResponse(ResponseOptions options, string param = "")
        {
            switch (options)
            {
                case ResponseOptions.Ok:
                    return Ok(param);

                case ResponseOptions.NotFound:
                    return NotFound();

                case ResponseOptions.Exists:
                    return BadRequest("Exists");

                default:
                    return BadRequest(param);
            }
        }
    }
}
