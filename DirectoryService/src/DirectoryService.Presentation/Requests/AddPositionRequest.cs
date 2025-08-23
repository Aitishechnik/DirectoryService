using DirectoryService.Application.Positions.Commands.Add;

namespace DirectoryService.Presentation.Requests
{
    public record AddPositionRequest(
        string Name,
        string? Description,
        List<Guid> DepartmentIds)
    {
        public AddPositionCommand ToCommand() => new(
            Name,
            Description,
            DepartmentIds);
    }
}