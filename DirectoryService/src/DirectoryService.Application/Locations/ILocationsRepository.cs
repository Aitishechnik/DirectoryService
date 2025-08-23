using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities.Locations;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Application.Locations
{
    public interface ILocationsRepository
    {
        Task<UnitResult<Error>> AddAsync(
            Location location,
            CancellationToken cancellationToken);

        Task<bool> IsLocationNameAvailibleAsync(
            string locationName,
            CancellationToken cancellationToken);

        Task<bool> IsLocationAddressExistsAsync(
            string state,
            string city,
            string address,
            CancellationToken cancellationToken);

        Task<Result<List<Location>, Error>> GetLocationsById(
            List<Guid> locationIds,
            CancellationToken cancellationToken);
    }
}