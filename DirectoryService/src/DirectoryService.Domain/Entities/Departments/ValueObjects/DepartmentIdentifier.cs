using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Departments.ValueObjects;

public record DepartmentIdentifier
{
    private DepartmentIdentifier(string identifier)
    {
        Identifier = identifier;
    }

    public string Identifier { get; }

    public static Result<DepartmentIdentifier, Error> Create(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier) ||
           !Regex.IsMatch(identifier, Constants.IDENTIFIER_REGEX_PATTERN))
            return GeneralErrors.ValueIsInvalid("Department Identifier");

        return new DepartmentIdentifier(identifier);
    }
}