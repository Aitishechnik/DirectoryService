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

        public static Result<LocationName, Error> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                name.Length < Constants.MIN_LOCATION_NAME_LENGTH ||
                name.Length > Constants.MAX_LOCATION_NAME_LENGTH)
                return GeneralErrors.ValueIsInvalid("LocationName");

            return new LocationName(name);
        }
    }
}