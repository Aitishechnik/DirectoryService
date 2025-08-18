using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Positions.ValueObjects
{
    public record PositionDescription
    {
        public PositionDescription(string description)
        {
            Description = description;
        }

        public string Description { get; }

        public static Result<PositionDescription, Error> Create(string description)
        {
            if (string.IsNullOrWhiteSpace(description) ||
                description.Length > Constants.MAX_POSITION_DESCRIPTION_LENGTH)
            {
                return GeneralErrors.ValueIsInvalid("Position description is invalid");
            }

            return new PositionDescription(description);
        }
    }
}