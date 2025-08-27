using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities.Departments;
using DirectoryService.Domain.Entities.Departments.ValueObjects;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Application.Departments
{
    public interface IDepartmentsRepository
    {
        Task<Result<Department, Error>> GetDepartmentById(
            Guid departmentId,
            CancellationToken cancellationToken);

        Task<UnitResult<Error>> AddAsync(
            Department department,
            CancellationToken cancellationToken);

        Task<Result<List<Department>, Error>> GetDepartmentsById(
            List<Guid> departmentIds,
            CancellationToken cancellationToken);

        Task<bool> IsIndentifierUnique(string departmentIdentifier);
    }
}