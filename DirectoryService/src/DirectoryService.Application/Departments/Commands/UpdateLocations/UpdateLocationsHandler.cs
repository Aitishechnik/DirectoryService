using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Validation;
using DirectoryService.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Departments.Commands.UpdateLocations
{
    public class UpdateLocationsHandler : IUpdateLocationsHandler
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IDepartmentsRepository _departmentRepository;
        private readonly ILocationsRepository _locationRepository;
        private readonly ILogger<UpdateLocationsHandler> _logger;
        private readonly IValidator<UpdateLocationsCommand> _validator;

        public UpdateLocationsHandler(
            ITransactionManager transactionManager,
            IDepartmentsRepository departmentRepository,
            ILocationsRepository locationRepository,
            ILogger<UpdateLocationsHandler> logger,
            IValidator<UpdateLocationsCommand> validator)
        {
            _transactionManager = transactionManager;
            _departmentRepository = departmentRepository;
            _locationRepository = locationRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<UnitResult<Errors>> Handle(
            UpdateLocationsCommand command,
            CancellationToken cancellationToken)
        {
            var validtnResult = await _validator.ValidateAsync(command);
            if (!validtnResult.IsValid)
            {
                var errors = validtnResult.ToList();
                errors.ToList().ForEach(
                    e => _logger.LogError(
                        "{code} {message} {type} {field}", e.Code, e.Message, e.Type, e.InvalidField));
                return errors;
            }

            var departmentResult = await _departmentRepository
                .GetDepartmentById(command.DepartmentId, cancellationToken);
            if (departmentResult.IsFailure)
            {
                _logger.LogError(departmentResult.Error.Message);
                return departmentResult.Error.ToErrors();
            }

            var locationsResult = await _locationRepository
                .GetLocationsById(command.LocationIds, cancellationToken);
            if (locationsResult.IsFailure)
            {
                _logger.LogError(locationsResult.Error.Message);
                return locationsResult.Error.ToErrors();
            }

            var transactionResult = await _transactionManager
                .BeginTransactionAsync(cancellationToken);
            if (transactionResult.IsFailure)
            {
                _logger.LogError(transactionResult.Error.Message);
                return transactionResult.Error.ToErrors();
            }

            using var transaction = transactionResult.Value;

            var updateLoctionsReuslt =
                departmentResult.Value.UpdateLocations(locationsResult.Value);
            if (updateLoctionsReuslt.IsFailure)
            {
                _logger.LogError(updateLoctionsReuslt.Error.Message);
                return updateLoctionsReuslt.Error.ToErrors();
            }

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
                "Locations for department with id '{DepartmentId}' were updated.",
                command.DepartmentId);

            return UnitResult.Success<Errors>();
        }
    }
}