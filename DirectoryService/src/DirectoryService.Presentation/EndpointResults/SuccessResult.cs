using System.Net;
using DirectoryService.Presentation.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DirectoryService.Presentation.EndpointResults
{
    public sealed class SuccessResult<TValue> : IResult
    {
        private readonly TValue _value;

        public SuccessResult(TValue value)
        {
            _value = value;
        }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            var envelope = Envelope.Ok(_value);

            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            var jsonOptions = httpContext.RequestServices
                .GetRequiredService<IOptions<JsonOptions>>()
                .Value.JsonSerializerOptions;

            await httpContext.Response.WriteAsJsonAsync(envelope, jsonOptions);
        }
    }
}