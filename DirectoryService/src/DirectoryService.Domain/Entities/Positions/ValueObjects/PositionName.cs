using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Positions.ValueObjects
{
    public record PositionName
    {
        private PositionName(string name)
        {
            Value = name;
        }

        public string Value { get; }

        public static Result<PositionName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                name.Length < Constants.MIN_POSITION_NAME_LENGTH ||
                name.Length > Constants.MAX_POSITION_NAME_LENGTH)
                return Result.Failure<PositionName>("Position name cannot be empty or whitespace.");
            return new PositionName(name);
        }
    }
}