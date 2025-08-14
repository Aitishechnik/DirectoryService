using DirectoryService.Domain.Entities.Locations;

namespace DirectoryService.Application.Locations
{
    public interface ILocationRepository
    {
        Task AddAsync(Location location, CancellationToken cancellationToken);

        Task<bool> IsLocationNameAvailibleAsync(
            string locationName,
            CancellationToken cancellationToken);

        Task<bool> IsLocationAddressExistsAsync(
            string state,
            string city,
            string address,
            CancellationToken cancellationToken);
    }
}