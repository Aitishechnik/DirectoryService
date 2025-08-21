using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Application.Positions.Commands.Add
{
    public interface IAddPositionHandler
    {
        Task<Result<Guid, Errors>> Handle(
            AddPositionCommand command,
            CancellationToken cancellationToken);
    }
}