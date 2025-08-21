using DirectoryService.Application.Departments.Commands.Add;
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
    }
}