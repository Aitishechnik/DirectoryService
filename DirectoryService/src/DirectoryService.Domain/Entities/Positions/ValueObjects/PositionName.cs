using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities.Positions.ValueObjects
{
    public record PositionName
    {
        private PositionName(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static Result<PositionName, Error> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                name.Length < Constants.MIN_POSITION_NAME_LENGTH ||
                name.Length > Constants.MAX_POSITION_NAME_LENGTH)
                return GeneralErrors.ValueIsRequired("Position name is invalid");

            return new PositionName(name);
        }
    }
}