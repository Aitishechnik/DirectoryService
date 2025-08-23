using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities.Positions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Application.Positions
{
    public interface IPositionsRepository
    {
        Task<UnitResult<Error>> AddAsync(
            Position position,
            CancellationToken cancellationToken);

        Task<bool> IsPositionExist(string name);
    }
}