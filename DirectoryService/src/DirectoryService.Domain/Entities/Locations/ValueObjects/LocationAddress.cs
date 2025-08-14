using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Entities.Locations.ValueObjects
{
    public record LocationAddress
    {
        private LocationAddress(
            string state,
            string city,
            string address)
        {
            State = state;
            City = city;
            Address = address;
        }

        public string State { get; }

        public string City { get; }

        public string Address { get; }

        public static Result<LocationAddress> Create(
            string state,
            string city,
            string address)
        {
            if(string.IsNullOrWhiteSpace(address))
                return Result.Failure<LocationAddress>($"{address} is not a valid address");
            if(string.IsNullOrWhiteSpace(city))
                return Result.Failure<LocationAddress>($"{city} is not a valid city");
            if(string.IsNullOrWhiteSpace(state))
                return Result.Failure<LocationAddress>($"{state} is not a valid state");

            return new LocationAddress(state, city, address);
        }
    }
}