namespace DirectoryService.Application.Departments.Commands.UpdateLocations
{
    public record UpdateLocationsCommand(
        Guid DepartmentId,
        List<Guid> LocationIds);
}