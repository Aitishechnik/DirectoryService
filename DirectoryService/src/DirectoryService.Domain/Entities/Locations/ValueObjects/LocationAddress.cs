using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

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

        public static Result<LocationAddress, Error> Create(
            string state,
            string city,
            string address)
        {
            if(string.IsNullOrWhiteSpace(address))
                return GeneralErrors.ValueIsRequired("Address is empty");
            if (string.IsNullOrWhiteSpace(city))
                return GeneralErrors.ValueIsRequired("City is empty");
            if (string.IsNullOrWhiteSpace(state))
                return GeneralErrors.ValueIsRequired("State is empty");

            return new LocationAddress(state, city, address);
        }
    }
}