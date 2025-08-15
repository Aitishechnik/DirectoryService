using DirectoryService.Application.Validation;
using DirectoryService.Domain.Entities.Locations.ValueObjects;
using FluentValidation;

namespace DirectoryService.Application.Locations.Add
{
    public class AddLocationCommandValidator : AbstractValidator<AddLocationCommand>
    {
        public AddLocationCommandValidator()
        {
            RuleFor(x => x.Name)
                .MustBeValueObject(x => LocationName.Create(x));

            RuleFor(x => x.LocationAddresDto)
                .MustBeValueObject(x => LocationAddress.Create(
                    x.State, x.City, x.Address));

            RuleFor(x => x.TimeZone)
                .MustBeValueObject(x => LocationTimeZone.Create(x));
        }
    }
}