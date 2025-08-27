using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Validation;
using DirectoryService.Domain.Entities.Departments;
using DirectoryService.Domain.Entities.Departments.ValueObjects;
using DirectoryService.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Departments.Commands.Add
{
    public class AddDepartmentHandler : IAddDepartmentHandler
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly ILogger<AddDepartmentHandler> _logger;
        private readonly IValidator<AddDepartmentCommand> _validator;

        public AddDepartmentHandler(
            ITransactionManager transactionManager,
            IDepartmentsRepository departmentsRepository,
            ILocationsRepository locationsRepository,
            ILogger<AddDepartmentHandler> logger,
            IValidator<AddDepartmentCommand> validator)
        {
            _transactionManager = transactionManager;
            _departmentsRepository = departmentsRepository;
            _locationsRepository = locationsRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<Guid, Errors>> Handle(
            AddDepartmentCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.ToList();

                var concatenatedErrors = string.Join("; ", errors.Select(
                    e => e.Code + " " + e.Message + " " + e.Type + " " + e.InvalidField));

                _logger.LogError(concatenatedErrors);

                return errors;
            }

            var locationsResult = await _locationsRepository
                    .GetLocationsById(
                        command.LocationIds,
                        cancellationToken);
            if (locationsResult.IsFailure)
            {
                _logger.LogError(locationsResult.Error.Message);
                return locationsResult.Error.ToErrors();
            }

            if(!await _departmentsRepository.IsIndentifierUnique(
                command.Identifier))
            {
                _logger.LogError("DepartmentIdentifier is not unique");

                return GeneralErrors
                    .AlreadyExist(command.Identifier, "DepartmentIdentifier")
                    .ToErrors();
            }

            Department department;

            Department? parentDepartment = default;

            var transactionResult = await _transactionManager
                .BeginTransactionAsync(cancellationToken);
            if (transactionResult.IsFailure)
            {
                _logger.LogError(transactionResult.Error.Message);
                return transactionResult.Error.ToErrors();
            }

            using var transaction = transactionResult.Value;

            if (command.ParentId is not null)
            {
                var parentDepartmentResult = await _departmentsRepository
                    .GetDepartmentById(
                        (Guid)command.ParentId,
                        cancellationToken);

                if (parentDepartmentResult.IsFailure)
                {
                    _logger.LogError(parentDepartmentResult.Error.Message);

                    return parentDepartmentResult.Error.ToErrors();
                }

                parentDepartment = parentDepartmentResult.Value;

                department = new Department(
                    DepartmentName.Create(command.Name).Value,
                    DepartmentIdentifier.Create(command.Identifier).Value,
                    command.ParentId,
                    DepartmentPath.Create(
                        parentDepartment.Path.Path + '.' + command.Identifier).Value,
                    (short)(parentDepartmentResult.Value.Depth + 1),
                    locationsResult.Value);
            }
            else
            {
                department = new Department(
                    DepartmentName.Create(command.Name).Value,
                    DepartmentIdentifier.Create(command.Identifier).Value,
                    null,
                    DepartmentPath.Create(command.Identifier).Value,
                    0,
                    locationsResult.Value);
            }

            var result = await _departmentsRepository
                .AddAsync(department, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogError(result.Error.Message);
                return result.Error.ToErrors();
            }

            if(parentDepartment is not null)
                parentDepartment.IncrementChildnenCount();

            var saveChangesResult = await _transactionManager.SaveChangesAsync(cancellationToken);
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
                "Department with ID {DepartmentId} was added successfully.",
                department.Id);

            return department.Id;
        }
    }
}