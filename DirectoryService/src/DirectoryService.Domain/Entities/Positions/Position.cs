using DirectoryService.Domain.Entities.Departments;
using DirectoryService.Domain.Entities.Positions.ValueObjects;

namespace DirectoryService.Domain.Entities.Positions;

public class Position
{
    public Position(
        PositionName name,
        PositionDescription? description)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    private Position() { }

    public Guid Id { get; private set; } = default!;

    public PositionName Name { get; private set; } = default!;

    public PositionDescription? Description { get; private set; }

    public IReadOnlyList<Department> Departments => _departments;

    private List<Department> _departments = [];

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
}