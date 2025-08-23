using CSharpFunctionalExtensions;
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
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ILocationsRepository _locationsRepository;
        private readonly ILogger<AddDepartmentHandler> _logger;
        private readonly IValidator<AddDepartmentCommand> _validator;

        public AddDepartmentHandler(
            IDepartmentsRepository departmentsRepository,
            ILocationsRepository locationsRepository,
            ILogger<AddDepartmentHandler> logger,
            IValidator<AddDepartmentCommand> validator)
        {
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

                errors.ToList().ForEach(
                    e => _logger.LogError(
                        "{code} {message} {type} {field}", e.Code, e.Message, e.Type, e.InvalidField));

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

            Department department;

            if (command.ParentId is not null)
            {
                var parentDepartment = await _departmentsRepository
                    .GetDepartmentById(
                        (Guid)command.ParentId,
                        cancellationToken);

                if (parentDepartment.IsFailure)
                {
                    _logger.LogError(parentDepartment.Error.Message);

                    return parentDepartment.Error.ToErrors();
                }

                if(!parentDepartment.Value
                    .IsIdentifierUniqueAmongChildren(
                    DepartmentIdentifier.Create(command.Identifier).Value))
                {
                    _logger.LogError("DepartmentIdentidier is not unique");
                    return GeneralErrors.AlreadyExist(
                        command.Identifier, "DepartmentIdentidier").ToErrors();
                }

                department = new Department(
                    DepartmentName.Create(command.Name).Value,
                    DepartmentIdentifier.Create(command.Identifier).Value,
                    command.ParentId,
                    DepartmentPath.Create(
                        parentDepartment.Value.Path.Path + '.' + command.Identifier).Value,
                    (short)(parentDepartment.Value.Depth + 1),
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

            _logger.LogInformation(
                "Department with ID {DepartmentId} was added successfully.",
                department.Id);

            return department.Id;
        }
    }
}