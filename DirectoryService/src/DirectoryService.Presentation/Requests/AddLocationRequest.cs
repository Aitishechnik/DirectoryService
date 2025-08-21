using DirectoryService.Application.Locations.Commands.Add;
using DirectoryService.Contracts.Dtos;

namespace DirectoryService.Presentation.Requests
{
    public record AddLocationRequest(
        string Name,
        string State,
        string City,
        string Address,
        string Timezone)
    {
        public AddLocationCommand ToCommand() =>
            new AddLocationCommand(
                Name,
                new LocationAddressDto(State, City, Address),
                Timezone);
    }
}