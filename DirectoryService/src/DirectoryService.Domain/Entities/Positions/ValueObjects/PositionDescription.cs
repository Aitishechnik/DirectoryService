using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Positions.ValueObjects
{
    public record PositionDescription
    {
        public PositionDescription(string description)
        {
            Value = description;
        }

        public string Value { get; }

        public static Result<PositionDescription> Create(string description)
        {
            if (string.IsNullOrWhiteSpace(description) ||
                description.Length > Constants.MAX_POSITION_DESCRIPTION_LENGTH)
            {
                return Result.Failure<PositionDescription>("Position description cannot be empty or whitespace.");
            }

            return new PositionDescription(description);
        }
    }
}