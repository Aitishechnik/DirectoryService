using System.Reflection;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;
using DirectoryService.Presentation.Response;
using Microsoft.AspNetCore.Http.Metadata;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace DirectoryService.Presentation.EndpointResults
{
    public abstract class EndpointResultBase : IResult, IEndpointMetadataProvider
    {
        private readonly IResult _result;

        protected EndpointResultBase(IResult result)
        {
            _result = result;
        }

        public Task ExecuteAsync(HttpContext httpContext) =>
            _result.ExecuteAsync(httpContext);

        public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(method);
            ArgumentNullException.ThrowIfNull(builder);

            builder.Metadata.Add(new ProducesResponseTypeMetadata(200, typeof(Envelope), ["application/json"]));
            builder.Metadata.Add(new ProducesResponseTypeMetadata(500, typeof(Envelope), ["application/json"]));
            builder.Metadata.Add(new ProducesResponseTypeMetadata(400, typeof(Envelope<Errors>), ["application/json"]));
            builder.Metadata.Add(new ProducesResponseTypeMetadata(404, typeof(Envelope<Errors>), ["application/json"]));
            builder.Metadata.Add(new ProducesResponseTypeMetadata(401, typeof(Envelope<Errors>), ["application/json"]));
            builder.Metadata.Add(new ProducesResponseTypeMetadata(403, typeof(Envelope<Errors>), ["application/json"]));
            builder.Metadata.Add(new ProducesResponseTypeMetadata(409, typeof(Envelope<Errors>), ["application/json"]));
        }
    }

    public sealed class EndpointResult<TValue> : EndpointResultBase
    {
        public EndpointResult(Result<TValue, Error> result)
            : base(result.IsSuccess
                ? new SuccessResult<TValue>(result.Value)
                : new ErrorsResult(result.Error))
        { }

        public EndpointResult(Result<TValue, Errors> result)
            : base(result.IsSuccess
                ? new SuccessResult<TValue>(result.Value)
                : new ErrorsResult(result.Error))
        { }

        public static implicit operator EndpointResult<TValue>(Result<TValue, Error> result) => new(result);

        public static implicit operator EndpointResult<TValue>(Result<TValue, Errors> result) => new(result);
    }

    public sealed class EndpointResult : EndpointResultBase
    {
        public EndpointResult(UnitResult<Error> result)
            : base(result.IsSuccess
                ? new SuccessResult()
                : new ErrorsResult(result.Error))
        { }

        public EndpointResult(UnitResult<Errors> result)
            : base(result.IsSuccess
                ? new SuccessResult()
                : new ErrorsResult(result.Error))
        { }

        public static implicit operator EndpointResult(UnitResult<Error> result) => new(result);

        public static implicit operator EndpointResult(UnitResult<Errors> result) => new(result);
    }
}