using CSharpFunctionalExtensions;
using DirectoryService.Application.Positions;
using DirectoryService.Domain.Entities.Positions;
using DirectoryService.Domain.Shared;
using DirectoryService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repositories
{
    public class PositionsRepository : IPositionsRepository
    {
        private readonly DirectoryServiceDbContext _dbContext;

        public PositionsRepository(DirectoryServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UnitResult<Error>> AddAsync(
            Position position,
            CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Positions.AddAsync(position, cancellationToken);

                return Result.Success<Error>();
            }
            catch (Exception ex)
            {
                return Error.Failure("fail.to.add.position", ex.Message);
            }
        }

        public async Task<bool> IsPositionExist(string name) =>
            await _dbContext.Positions.AnyAsync(p => p.Name.Name == name);
    }
}