using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Application.Departments.Commands.Add
{
    public interface IAddDepartmentHandler
    {
        Task<Result<Guid, Errors>> Handle(
            AddDepartmentCommand command,
            CancellationToken cancellationToken);
    }
}