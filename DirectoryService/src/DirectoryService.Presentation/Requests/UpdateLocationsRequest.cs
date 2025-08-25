using DirectoryService.Application.Departments.Commands.UpdateLocations;

namespace DirectoryService.Presentation.Requests
{
    public record UpdateLocationsRequest(List<Guid> LocationIds)
    {
        public UpdateLocationsCommand ToCommand(Guid departmentId) =>
            new(departmentId, LocationIds);
    }
}