using DirectoryService.Application.Validation;
using DirectoryService.Domain.Entities.Positions.ValueObjects;
using DirectoryService.Domain.Shared;
using FluentValidation;

namespace DirectoryService.Application.Positions.Commands.Add
{
    public class AddPositionCommandValidator : AbstractValidator<AddPositionCommand>
    {
        public AddPositionCommandValidator()
        {
            RuleFor(x => x.Name)
                .MustBeValueObject(name => PositionName.Create(name));

            RuleFor(x => x.Description)
                .MustBeValueObject(description => PositionDescription.Create(description!))
                .When(x => x.Description != null);

            RuleFor(x => x.DepartmentIds)
                .NotEmpty()
                .WithError(GeneralErrors.ValueIsRequired("DepartmentIds"))
                .Must(x => x.Distinct().Count() == x.Count)
                .WithError(GeneralErrors.ValuesAreNotDistinct("DepartmentIds"));
        }
    }
}