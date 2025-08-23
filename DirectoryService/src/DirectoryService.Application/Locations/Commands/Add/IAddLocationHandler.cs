using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Application.Locations.Commands.Add
{
    public interface IAddLocationHandler
    {
        Task<Result<Guid, Errors>> Handle(
            AddLocationCommand command,
            CancellationToken cancellationToken);
    }
}