using CSharpFunctionalExtensions;

namespace DirectoryService.Application.Locations.Add
{
    public interface IAddLocationHandler
    {
        Task<Result<Guid>> Handle(
            AddLocationCommand command,
            CancellationToken cancellationToken);
    }
}