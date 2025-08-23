using DirectoryService.Application.Positions.Commands.Add;
using DirectoryService.Presentation.EndpointResults;
using DirectoryService.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers
{
    [ApiController]
    [Route("api/positions")]
    public class PositionsController : ControllerBase
    {
        [HttpPost]
        public async Task<EndpointResult<Guid>> AddPosition(
            [FromBody] AddPositionRequest request,
            [FromServices] IAddPositionHandler handler,
            CancellationToken cancellationToken)
        {
            return await handler.Handle(request.ToCommand(), cancellationToken);
        }
    }
}