using Microsoft.AspNetCore.Mvc;

namespace Tripex.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult CheckResponse(ResponseOptions options)
        {
            switch (options)
            {
                case ResponseOptions.Ok:
                    return Ok();
                    break;

                case ResponseOptions.NotFound:
                    return NotFound();
                    break;

                case ResponseOptions.Exists:
                    return BadRequest("Exists");
                    break;

                default:
                    return BadRequest();
            }
        }
    }
}
