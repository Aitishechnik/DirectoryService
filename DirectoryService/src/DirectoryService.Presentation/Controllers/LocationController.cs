using DirectoryService.Application.Locations.Add;
using DirectoryService.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddLocation(
            [FromBody] AddLocationRequest request,
            [FromServices] IAddLocationHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(
                request.ToCommand(),
                cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}