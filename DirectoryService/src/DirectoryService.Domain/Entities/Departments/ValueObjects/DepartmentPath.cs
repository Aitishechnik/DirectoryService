using CSharpFunctionalExtensions;

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
            if (string.IsNullOrWhiteSpace(path) || path.Length < 1 || path.Length > 255)
                return Result.Failure<DepartmentPath>("Path should not be empty or white space and must be between 1 and 255 characters.");
            return Result.Success(new DepartmentPath(path));
        }
    }
}