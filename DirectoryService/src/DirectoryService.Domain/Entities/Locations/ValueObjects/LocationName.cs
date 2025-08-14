using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Locations.ValueObjects
{
    public record LocationName
    {
        private LocationName(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static Result<LocationName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                name.Length < Constants.MIN_LOCATION_NAME_LENGTH ||
                name.Length > Constants.MAX_LOCATION_NAME_LENGTH)
                return Result.Failure<LocationName>("Name is empty.");

            return new LocationName(name);
        }
    }
}