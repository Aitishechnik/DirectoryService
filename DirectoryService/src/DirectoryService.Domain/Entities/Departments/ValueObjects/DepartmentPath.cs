using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Departments.ValueObjects
{
    public class DepartmentPath
    {
        private DepartmentPath(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<DepartmentPath> Create(string path)
        {
            if (string.IsNullOrWhiteSpace(path) ||
                !Regex.IsMatch(path, Constants.DEPARTMENT_PATH_REGEX_PATTERN))
                return Result.Failure<DepartmentPath>("Incorrect path format.");
            return Result.Success(new DepartmentPath(path));
        }
    }
}