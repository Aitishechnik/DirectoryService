using CSharpFunctionalExtensions;
using FluentValidation;

namespace DirectoryService.Application.Validation
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject>> factoyMethod)
        {
            return ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject> result = factoyMethod(value);

                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error);
            });
        }
    }
}