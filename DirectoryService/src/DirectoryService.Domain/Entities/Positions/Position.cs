using DirectoryService.Domain.Entities.Departments;
using DirectoryService.Domain.Entities.Positions.ValueObjects;

namespace DirectoryService.Domain.Entities.Positions;

public class Position
{
    public Guid Id { get; private set; }

    public PositionName Name { get; private set; }

    public PositionDescription? Description { get; private set; }

    public IReadOnlyList<Department> Departments => _departments;

    private List<Department> _departments = [];

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public Position(
        PositionName name,
        PositionDescription? description,
        bool isActive)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}