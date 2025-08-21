using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.Entities.Departments;
using DirectoryService.Domain.Shared;
using DirectoryService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repositories
{
    public class DepartmentsRepositoriy : IDepartmentsRepository
    {
        private readonly DirectoryServiceDbContext _dbContext;

        public DepartmentsRepositoriy(DirectoryServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UnitResult<Error>> AddAsync(
            Department department,
            CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.AddAsync(department, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success<Error>();
            }
            catch (Exception ex)
            {
                return Error.Failure("fail.to.add.department", ex.Message);
            }
        }

        public async Task<UnitResult<Error>> SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success<Error>();
            }
            catch (Exception ex)
            {
                return Error.Failure("fail.to.update.department", ex.Message);
            }
        }

        public async Task<Result<Department, Error>> GetDepartmentById(
            Guid departmentId,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext
                .Departments
                .Include(d => d.Locations)
                .Include(d => d.Positions)
                .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);
            if(result is null)
                return GeneralErrors.ValueIsInvalid("DepartmentId");

            return result;
        }

        public async Task<Result<List<Department>, Error>> GetDepartmentsById(
            List<Guid> departmentIds,
            CancellationToken cancellationToken)
        {
            List<Department> departments = [];

            foreach(var id in departmentIds)
            {
                var departmentResult = await GetDepartmentById(id, cancellationToken);

                if (departmentResult.IsFailure)
                    return GeneralErrors.NotFound(id);
                else
                    departments.Add(departmentResult.Value);
            }

            return departments;
        }
    }
}