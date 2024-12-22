using Microsoft.AspNetCore.Mvc;

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
                    break;

                case ResponseOptions.NotFound:
                    return NotFound();
                    break;

                case ResponseOptions.Exists:
                    return BadRequest("Exists");
                    break;

                default:
                    return BadRequest(param);
            }
        }
    }
}
