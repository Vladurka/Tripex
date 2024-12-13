using Microsoft.AspNetCore.Mvc;

namespace Tripex.Presentation.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult CheckResponse(ResponseOptions options)
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
