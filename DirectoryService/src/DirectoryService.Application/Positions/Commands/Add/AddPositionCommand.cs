namespace DirectoryService.Application.Positions.Commands.Add
{
    public record AddPositionCommand(
        string Name,
        string? Description,
        List<Guid> DepartmentIds);
}