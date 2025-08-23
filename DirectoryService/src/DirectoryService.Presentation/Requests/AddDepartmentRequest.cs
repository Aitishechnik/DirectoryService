using DirectoryService.Application.Departments.Commands.Add;

namespace DirectoryService.Presentation.Requests
{
    public record AddDepartmentRequest(
        string Name,
        string Identifier,
        Guid? ParentId,
        List<Guid> LocationIds)
    {
        public AddDepartmentCommand ToCommand() => new(
            Name,
            Identifier,
            ParentId,
            LocationIds);
    }
}