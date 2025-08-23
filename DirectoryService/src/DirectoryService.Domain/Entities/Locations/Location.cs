using DirectoryService.Domain.Entities.Departments;
using DirectoryService.Domain.Entities.Locations.ValueObjects;

namespace DirectoryService.Domain.Entities.Locations;

public class Location
{
    public Location(
        LocationName name,
        LocationAddress address,
        LocationTimeZone timezone)
    {
        Name = name;
        Address = address;
        Timezone = timezone;
        CreatedAt = DateTime.UtcNow;
    }

    private Location() { }

    public Guid Id { get; private set; } = default!;

    public LocationName Name { get; private set; } = default!;

    public LocationAddress Address { get; private set; } = default!;

    public LocationTimeZone Timezone { get; private set; } = default!;

    public IReadOnlyList<Department> Departments => _departments;

    private List<Department> _departments = [];

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
}