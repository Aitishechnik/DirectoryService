using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Departments.ValueObjects
{
    public record DepartmentName
    {
        private DepartmentName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<DepartmentName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                name.Length < Constants.MIN_DEPARTMENT_NAME_LENGTH || 
                name.Length > Constants.MAX_DEPARTMENT_NAME_LENGTH)
                return Result.Success(new DepartmentName(name));

            return Result.Failure<DepartmentName>("Name should not be empty or white space.");
        }
    }
}