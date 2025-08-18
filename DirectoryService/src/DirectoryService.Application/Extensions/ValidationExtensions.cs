using DirectoryService.Domain.Shared;
using FluentValidation.Results;

namespace DirectoryService.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static Errors ToErrors(
            this ValidationResult validationResult) =>
            validationResult.Errors
                .Select(error => Error.Validation(
                    error.ErrorCode,
                    error.ErrorMessage,
                    error.PropertyName)).ToArray();
    }
}