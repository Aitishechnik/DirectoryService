using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Application.Departments.Commands.UpdateLocations
{
    public interface IUpdateLocationsHandler
    {
        Task<UnitResult<Errors>> Handle(
            UpdateLocationsCommand command,
            CancellationToken cancellationToken);
    }
}