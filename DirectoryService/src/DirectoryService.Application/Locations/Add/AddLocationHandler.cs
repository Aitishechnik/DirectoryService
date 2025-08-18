using CSharpFunctionalExtensions;
using DirectoryService.Application.Extensions;
using DirectoryService.Domain.Entities.Locations;
using DirectoryService.Domain.Entities.Locations.ValueObjects;
using DirectoryService.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Locations.Add
{
    public class AddLocationHandler : IAddLocationHandler
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<AddLocationHandler> _logger;
        private readonly IValidator<AddLocationCommand> _validator;

        public AddLocationHandler(
            ILocationRepository locationRepository,
            ILogger<AddLocationHandler> logger,
            IValidator<AddLocationCommand> validator)
        {
            _locationRepository = locationRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<Guid, Errors>> Handle(
            AddLocationCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await _validator
                .ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                validationResult.Errors
                    .ForEach(e =>
                    _logger.LogError(e.ErrorMessage));

                return validationResult.ToErrors();
            }

            var isLocationNameAvailable = await _locationRepository
                .IsLocationNameAvailibleAsync(
                    command.Name,
                    cancellationToken);
            if (!isLocationNameAvailable)
            {
                _logger.LogError(
                    "Location name '{LocationName}' is already taken.",
                    command.Name);

                return GeneralErrors.AlreadyExist(command.Name, "name")
                    .ToErrors();
            }

            var isLocationAddressExists = await _locationRepository
                .IsLocationAddressExistsAsync(
                    command.LocationAddresDto.State,
                    command.LocationAddresDto.City,
                    command.LocationAddresDto.Address,
                    cancellationToken);
            if (isLocationAddressExists)
            {
                _logger.LogError(
                    "Location address '{State}, {City}, {Address}' already exists.",
                    command.LocationAddresDto.State,
                    command.LocationAddresDto.City,
                    command.LocationAddresDto.Address);

                return GeneralErrors.AlreadyExist(
                    $"{command.LocationAddresDto.State} " +
                    $"{command.LocationAddresDto.City} " +
                    $"{command.LocationAddresDto.Address}", "adress").ToErrors();
            }

            var location = new Location(
                LocationName.Create(command.Name).Value,
                LocationAddress.Create(
                    command.LocationAddresDto.State,
                    command.LocationAddresDto.City,
                    command.LocationAddresDto.Address).Value,
                LocationTimeZone.Create(command.TimeZone).Value,
                DateTime.UtcNow);

            var result = await _locationRepository
                .AddAsync(location, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToErrors();

            return location.Id;
        }
    }
}