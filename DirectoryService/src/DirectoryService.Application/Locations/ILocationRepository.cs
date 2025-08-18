using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities.Locations;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Application.Locations
{
    public interface ILocationRepository
    {
        Task<UnitResult<Error>> AddAsync(Location location, CancellationToken cancellationToken);

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