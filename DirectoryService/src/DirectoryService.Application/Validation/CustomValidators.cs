using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;
using FluentValidation;

namespace DirectoryService.Application.Validation
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject, Error>> factoyMethod)
        {
            return ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoyMethod(value);

                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error.Serialize());
            });
        }

        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, Error error)
        {
            return rule.WithMessage(error.Serialize());
        }
    }
}