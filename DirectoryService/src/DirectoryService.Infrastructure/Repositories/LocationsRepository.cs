using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.Entities.Locations;
using DirectoryService.Domain.Shared;
using DirectoryService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repositories
{
    public class LocationsRepository : ILocationsRepository
    {
        private readonly DirectoryServiceDbContext _dbContext;

        public LocationsRepository(DirectoryServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UnitResult<Error>> AddAsync(
            Location location,
            CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Locations.AddAsync(location, cancellationToken);

                return UnitResult.Success<Error>();
            }
            catch (Exception ex)
            {
                return Error.Failure(null, ex.Message);
            }
        }

        public async Task<Result<List<Location>, Error>> GetLocationsById(
            List<Guid> locationsIds,
            CancellationToken cancellationToken)
        {
            var distinctLocations = locationsIds.Distinct().ToList();

            var locations = await _dbContext.Locations
                .Where(l => distinctLocations.Contains(l.Id))
                .ToListAsync(cancellationToken);

            if (distinctLocations.Count != locations.Count)
                return GeneralErrors.ValueIsInvalid("LocationName");

            return locations;
        }

        public async Task<bool> IsLocationAddressExistsAsync(
            string state,
            string city,
            string address,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Locations
                .AnyAsync(
                    l => l.Address.State == state &&
                        l.Address.City == city &&
                        l.Address.Address == address,
                    cancellationToken);
        }

        public async Task<bool> IsLocationNameAvailibleAsync(
            string locationName,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Locations
                .AllAsync(
                    l => l.Name.Name != locationName,
                    cancellationToken);
        }
    }
}