using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Locations.ValueObjects
{
    public record LocationTimeZone
    {
        private LocationTimeZone(string timeZone)
        {
            TimeZone = timeZone;
        }

        public string TimeZone { get; }

        public static Result<LocationTimeZone, Error> Create(string timeZone)
        {
            if (string.IsNullOrWhiteSpace(timeZone) ||
                !Regex.IsMatch(timeZone, Constants.TIME_ZONE_REGEX_PATTERN))
                return GeneralErrors.ValueIsInvalid("Time zone is invalid");

            return new LocationTimeZone(timeZone);
        }
    }
}