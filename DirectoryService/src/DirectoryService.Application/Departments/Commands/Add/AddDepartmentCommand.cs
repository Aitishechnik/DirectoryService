namespace DirectoryService.Application.Departments.Commands.Add
{
    public record AddDepartmentCommand(
        string Name,
        string Identifier,
        Guid? ParentId,
        List<Guid> LocationIds);
}