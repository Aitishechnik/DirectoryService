using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Validation;
using DirectoryService.Domain.Entities.Locations;
using DirectoryService.Domain.Entities.Locations.ValueObjects;
using DirectoryService.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Locations.Commands.Add
{
    public class AddLocationHandler : IAddLocationHandler
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ILocationsRepository _locationRepository;
        private readonly ILogger<AddLocationHandler> _logger;
        private readonly IValidator<AddLocationCommand> _validator;

        public AddLocationHandler(
            ITransactionManager transactionManager,
            ILocationsRepository locationRepository,
            ILogger<AddLocationHandler> logger,
            IValidator<AddLocationCommand> validator)
        {
            _transactionManager = transactionManager;
            _locationRepository = locationRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<Guid, Errors>> Handle(
            AddLocationCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.ToList();

                var concatenatedErrors = string.Join("; ", errors.Select(
                    e => e.Code + " " + e.Message + " " + e.Type + " " + e.InvalidField));

                _logger.LogError(concatenatedErrors);

                return errors;
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
                LocationTimeZone.Create(command.TimeZone).Value);

            var transactionResult = await _transactionManager
                .BeginTransactionAsync(cancellationToken);
            if (transactionResult.IsFailure)
            {
                _logger.LogError(transactionResult.Error.Message);
                return transactionResult.Error.ToErrors();
            }

            using var transaction = transactionResult.Value;

            var result = await _locationRepository
                .AddAsync(location, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToErrors();

            var saveChangesResult = await _transactionManager
                .SaveChangesAsync(cancellationToken);
            if (saveChangesResult.IsFailure)
            {
                _logger.LogError(saveChangesResult.Error.Message);
                return saveChangesResult.Error.ToErrors();
            }

            var commitResult = transaction.Commit();
            if (commitResult.IsFailure)
            {
                _logger.LogError(commitResult.Error.Message);
                return commitResult.Error.ToErrors();
            }

            _logger.LogInformation(
                "Location '{LocationName}' with ID '{LocationId}' was created.",
                location.Name,
                location.Id);

            return location.Id;
        }
    }
}