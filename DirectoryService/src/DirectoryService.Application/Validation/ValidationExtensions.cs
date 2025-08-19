using DirectoryService.Domain.Shared;
using FluentValidation.Results;

namespace DirectoryService.Application.Validation
{
    public static class ValidationExtensions
    {
        public static Errors ToList(this ValidationResult validationResult)
        {
            var validationErrors = validationResult.Errors;

            var errors = validationErrors
                .Select(e =>
                {
                    var error = Error.Deserialize(e.ErrorMessage);
                    return Error.Validation(
                        error.Code,
                        error.Message,
                        e.PropertyName);
                }).ToList();

            return new Errors(errors);
        }
    }
}