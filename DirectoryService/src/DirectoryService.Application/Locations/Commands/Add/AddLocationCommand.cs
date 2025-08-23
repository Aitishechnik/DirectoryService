using DirectoryService.Contracts.Dtos;

namespace DirectoryService.Application.Locations.Commands.Add
{
    public record AddLocationCommand(
        string Name,
        LocationAddressDto LocationAddresDto,
        string TimeZone);
}