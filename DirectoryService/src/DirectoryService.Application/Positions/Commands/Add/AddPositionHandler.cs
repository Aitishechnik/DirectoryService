using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Validation;
using DirectoryService.Domain.Entities.Positions;
using DirectoryService.Domain.Entities.Positions.ValueObjects;
using DirectoryService.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Positions.Commands.Add
{
    public class AddPositionHandler : IAddPositionHandler
    {
        private readonly IPositionsRepository _positionRepository;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ILogger<AddPositionHandler> _logger;
        private readonly IValidator<AddPositionCommand> _validator;

        public AddPositionHandler(
            IPositionsRepository positionRepository,
            IDepartmentsRepository departmentsRepository,
            ILogger<AddPositionHandler> logger,
            IValidator<AddPositionCommand> validator)
        {
            _positionRepository = positionRepository;
            _departmentsRepository = departmentsRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<Guid, Errors>> Handle(
            AddPositionCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.ToList();

                errors.ToList().ForEach(
                    e => _logger.LogError(
                        "{code} {message} {type} {field}", e.Code, e.Message, e.Type, e.InvalidField));

                return errors;
            }

            if(await _positionRepository.IsPositionExist(command.Name))
            {
                _logger.LogError("Position with {Name} already exist", command.Name);

                return GeneralErrors.AlreadyExist("command name", command.Name).ToErrors();
            }

            var departmentsResult = await _departmentsRepository
                .GetDepartmentsById(command.DepartmentIds, cancellationToken);
            if (departmentsResult.IsFailure)
            {
                _logger.LogError(departmentsResult.Error.Message);
                return departmentsResult.Error.ToErrors();
            }

            var positionDescription = command.Description is not null
                ? PositionDescription.Create(command.Description).Value
                : null;

            var position = new Position(
                PositionName.Create(command.Name).Value,
                positionDescription);

            var result = await _positionRepository.AddAsync(position, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogError(result.Error.Message);
                return result.Error.ToErrors();
            }

            departmentsResult.Value.ForEach(
                d => d.SetPositions([position]));

            var updateDepartmentsResult = await _departmentsRepository
                .SaveChangesAsync(cancellationToken);
            if (updateDepartmentsResult.IsFailure)
            {
                _logger.LogError(updateDepartmentsResult.Error.Message);
                return updateDepartmentsResult.Error.ToErrors();
            }

            return position.Id;
        }
    }
}