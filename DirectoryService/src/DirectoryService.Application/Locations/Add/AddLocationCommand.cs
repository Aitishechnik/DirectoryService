using DirectoryService.Contracts.Dtos;

namespace DirectoryService.Application.Locations.Add
{
    public record AddLocationCommand(
        string Name,
        LocationAddressDto LocationAddresDto,
        string TimeZone);
}