using DirectoryService.Domain.Shared;
using DirectoryService.Presentation.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static DirectoryService.Domain.Shared.Error;

namespace DirectoryService.Presentation.EndpointResults
{
    public sealed class ErrorsResult : IResult
    {
        private readonly Errors _errors;

        public ErrorsResult(Error error)
        {
            _errors = error.ToErrors();
        }

        public ErrorsResult(Errors errors)
        {
            _errors = errors;
        }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            var jsonOptions = httpContext.RequestServices
                .GetRequiredService<IOptions<JsonOptions>>()
                .Value.JsonSerializerOptions;

            if (!_errors.Any())
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(Envelope.Error(_errors), jsonOptions);
                return;
            }

            var distinctErrorTypes = _errors.Select(e => e.Type).Distinct().ToList();

            int statusCode = distinctErrorTypes.Count > 1
                ? StatusCodes.Status500InternalServerError
                : GetStatusCodeForErrorType(distinctErrorTypes.First());

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(Envelope.Error(_errors), jsonOptions);
        }

        private static int GetStatusCodeForErrorType(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.VALIDATION => StatusCodes.Status400BadRequest,
                ErrorType.NOT_FOUND => StatusCodes.Status404NotFound,
                ErrorType.CONFLICT => StatusCodes.Status409Conflict,
                ErrorType.FAILURE => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };
    }
}