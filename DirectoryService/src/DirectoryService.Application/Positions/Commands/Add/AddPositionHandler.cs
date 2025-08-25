using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
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
        private readonly ITransactionManager _transactionManager;
        private readonly IPositionsRepository _positionRepository;
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly ILogger<AddPositionHandler> _logger;
        private readonly IValidator<AddPositionCommand> _validator;

        public AddPositionHandler(
            ITransactionManager transactionManager,
            IPositionsRepository positionRepository,
            IDepartmentsRepository departmentsRepository,
            ILogger<AddPositionHandler> logger,
            IValidator<AddPositionCommand> validator)
        {
            _transactionManager = transactionManager;
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

            var transactionResult = await _transactionManager
                .BeginTransactionAsync(cancellationToken);
            if (transactionResult.IsFailure)
            {
                _logger.LogError(transactionResult.Error.Message);
                return transactionResult.Error.ToErrors();
            }

            using var transaction = transactionResult.Value;

            var result = await _positionRepository.AddAsync(position, cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogError(result.Error.Message);
                return result.Error.ToErrors();
            }

            departmentsResult.Value.ForEach(
                d => d.AddPositions([position]));

            var updateDepartmentsResult = await _transactionManager
                .SaveChangesAsync(cancellationToken);
            if (updateDepartmentsResult.IsFailure)
            {
                _logger.LogError(updateDepartmentsResult.Error.Message);
                return updateDepartmentsResult.Error.ToErrors();
            }

            var commitResult = transaction.Commit();
            if (commitResult.IsFailure)
            {
                _logger.LogError(commitResult.Error.Message);
                return commitResult.Error.ToErrors();
            }

            _logger.LogInformation("Position with id '{PositionId}' was created", position.Id);

            return position.Id;
        }
    }
}