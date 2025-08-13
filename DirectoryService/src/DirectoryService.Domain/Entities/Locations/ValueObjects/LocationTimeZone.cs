using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Locations.ValueObjects
{
    public record LocationTimeZone
    {
        private LocationTimeZone(string timeZone)
        {
            Value = timeZone;
        }

        public string Value { get; }

        public static Result<LocationTimeZone> Create(string timeZone)
        {
            if (string.IsNullOrWhiteSpace(timeZone) ||
                !Regex.IsMatch(timeZone, Constants.TIME_ZONE_REGEX))
                return Result.Failure<LocationTimeZone>("Incorrect TimeZone format.");

            return new LocationTimeZone(timeZone);
        }
    }
}