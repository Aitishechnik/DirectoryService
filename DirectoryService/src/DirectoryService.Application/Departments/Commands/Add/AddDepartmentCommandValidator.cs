using DirectoryService.Application.Validation;
using DirectoryService.Domain.Entities.Departments.ValueObjects;
using DirectoryService.Domain.Shared;
using FluentValidation;

namespace DirectoryService.Application.Departments.Commands.Add
{
    public class AddDepartmentCommandValidator : AbstractValidator<AddDepartmentCommand>
    {
        public AddDepartmentCommandValidator()
        {
            RuleFor(x => x.Name)
                .MustBeValueObject(name => DepartmentName.Create(name));

            RuleFor(x => x.Identifier)
                .MustBeValueObject(identifier => DepartmentIdentifier.Create(identifier));

            RuleFor(x => x.ParentId).
                Must(id => id == null || id != Guid.Empty)
                .WithError(GeneralErrors.ValueIsInvalid("ParentId"));

            RuleFor(x => x.LocationIds)
                .NotEmpty()
                .WithError(GeneralErrors.ValueIsRequired("LocationIds"))
                .Must(x => x.Distinct().Count() == x.Count)
                .WithError(GeneralErrors.ValuesAreNotDistinct("LocationIds"));
        }
    }
}