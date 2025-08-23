using DirectoryService.Application.Locations.Commands.Add;
using DirectoryService.Presentation.EndpointResults;
using DirectoryService.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {
        [HttpPost]
        public async Task<EndpointResult<Guid>> AddLocation(
            [FromBody] AddLocationRequest request,
            [FromServices] IAddLocationHandler handler,
            CancellationToken cancellationToken)
        {
            return await handler.Handle(request.ToCommand(), cancellationToken);
        }
    }
}