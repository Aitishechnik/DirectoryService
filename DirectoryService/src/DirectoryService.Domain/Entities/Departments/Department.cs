using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities.Departments.ValueObjects;
using DirectoryService.Domain.Entities.Locations;
using DirectoryService.Domain.Entities.Positions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Departments;

public class Department
{
    public Department(
        DepartmentName name,
        DepartmentIdentifier identifier,
        Guid? parentId,
        DepartmentPath path,
        short debth,
        List<Location> locatoins)
    {
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = debth;
        ChildrenCount = _children.Count;
        _locations = locatoins;
        CreatedAt = DateTime.UtcNow;
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

    public int ChildrenCount { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public UnitResult<Error> AddLocations(List<Location> locations)
    {
        if(locations is null || locations.Count == 0)
            return GeneralErrors.ValueIsInvalid("Locations");

        if(_locations.Count == 0)
        {
            _locations = locations;
            return Result.Success<Error>();
        }

        var repitableLocation = locations.FirstOrDefault(
            l => _locations.Contains(l));

        if (repitableLocation is not null)
        {
            return GeneralErrors.AlreadyExist(
                repitableLocation.Name.Name, "Location");
        }

        _locations.AddRange(locations);

        return Result.Success<Error>();
    }

    public UnitResult<Error> UpdateLocations(List<Location> locations)
    {
         if (locations is null || locations.Count == 0)
            return GeneralErrors.ValueIsInvalid("Locations");

         _locations = locations;

         return Result.Success<Error>();
    }

    public UnitResult<Error> AddPositions(
        List<Position> positions)
    {
        if(_positions.Count == 0)
        {
            _positions = positions;
            return Result.Success<Error>();
        }

        var repitiblePosition = positions.FirstOrDefault(
            l => _positions.Contains(l));

        if(repitiblePosition is not null)
        {
            return GeneralErrors.AlreadyExist(
                repitiblePosition.Name.Name, "Position");
        }

        _positions.AddRange(positions);

        return Result.Success<Error>();
    }

    public void IncrementChildnenCount() => ChildrenCount++;

    public bool IsIdentifierUniqueAmongChildren(
        DepartmentIdentifier departmentIdentifier)
    {
        return _children.All(
            d => d.Identifier != departmentIdentifier);
    }
}