using DirectoryService.Application.Validation;
using DirectoryService.Domain.Shared;
using FluentValidation;

namespace DirectoryService.Application.Departments.Commands.UpdateLocations
{
    public class UpdateCommandHandlerValidator : AbstractValidator<UpdateLocationsCommand>
    {
        public UpdateCommandHandlerValidator()
        {
            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithError(GeneralErrors.ValueIsRequired("DepartmentId"));

            RuleFor(x => x.LocationIds)
                .NotEmpty()
                .WithError(GeneralErrors.ValueIsRequired("LocationIds"))
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithError(GeneralErrors.ValuesAreNotDistinct("LocationIds"));
        }
    }
}