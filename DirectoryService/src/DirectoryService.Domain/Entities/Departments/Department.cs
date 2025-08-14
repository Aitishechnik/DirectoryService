using DirectoryService.Domain.Entities.Departments.ValueObjects;
using DirectoryService.Domain.Entities.Locations;
using DirectoryService.Domain.Entities.Positions;

namespace DirectoryService.Domain.Entities.Departments;

public class Department
{
    public Department(
        DepartmentName name,
        DepartmentIdentifier identifier,
        Guid? parentId,
        DepartmentPath path,
        short debth,
        int childerCount,
        DateTime createAt,
        DateTime updateAt)
    {
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = debth;
        ChildrenCount = childerCount;
        CreatedAt = createAt;
        UpdatedAt = updateAt;
    }

    private Department() { }

    public Guid Id { get; private set; } = default!;

    public DepartmentName Name { get; private set; } = default!;

    public DepartmentIdentifier Identifier { get; private set; } = default!;

    public Guid? ParentId { get; private set; } = default!;

    public DepartmentPath Path { get; private set; } = default!;

    public short Depth { get; private set; } = default!;

    public IReadOnlyList<Department> Children => _children;

    private List<Department> _children = [];

    public IReadOnlyList<Location> Locations => _locations;

    private List<Location> _locations = [];

    public IReadOnlyList<Position> Positions => _positions;

    private List<Position> _positions = [];

    public int ChildrenCount { get; private set; } = default!;

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
}