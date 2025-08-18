using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Departments.ValueObjects
{
    public record DepartmentName
    {
        private DepartmentName(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static Result<DepartmentName, Error> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                name.Length < Constants.MIN_DEPARTMENT_NAME_LENGTH ||
                name.Length > Constants.MAX_DEPARTMENT_NAME_LENGTH)
                return GeneralErrors.ValueIsInvalid("Department Name");

            return new DepartmentName(name);
        }
    }
}