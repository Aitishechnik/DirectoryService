using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Entities.Locations.ValueObjects
{
    public record LocationAddress
    {
        private LocationAddress(string address)
        {
            Value = address;
        }

        public string Value { get; }

        public static Result<LocationAddress> Create(string address)
        {
            if(string.IsNullOrWhiteSpace(address))
                return Result.Failure<LocationAddress>($"{address} is not a valid address");

            return new LocationAddress(address);
        }
    }
}