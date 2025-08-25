using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments.Commands.Add;
using DirectoryService.Application.Departments.Commands.UpdateLocations;
using DirectoryService.Domain.Shared;
using DirectoryService.Presentation.EndpointResults;
using DirectoryService.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentsController : ControllerBase
    {
        [HttpPost]
        public async Task<EndpointResult<Guid>> AddDepartment(
            [FromBody] AddDepartmentRequest request,
            [FromServices] IAddDepartmentHandler handler,
            CancellationToken cancellationToken)
        {
            return await handler.Handle(request.ToCommand(), cancellationToken);
        }

        [HttpPut]
        [Route("{departmentId:guid}/locations")]
        public async Task<EndpointResult> UpdateLocations(
            [FromRoute] Guid departmentId,
            [FromBody] UpdateLocationsRequest request,
            [FromServices] IUpdateLocationsHandler handler,
            CancellationToken cancellationToken)
        {
            return await handler.Handle(request.ToCommand(departmentId), cancellationToken);
        }
    }
}