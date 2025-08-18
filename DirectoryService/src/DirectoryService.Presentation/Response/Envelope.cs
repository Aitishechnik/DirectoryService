using DirectoryService.Domain.Shared;
using System.Text.Json.Serialization;

namespace DirectoryService.Presentation.Response
{
    public record Envelope
    {
        public object? Result { get; }

        public Errors? Errors { get; }

        public bool IsError => Errors != null ||
            (Errors != null && Errors.Any());

        public DateTime TimeGenerated { get; }

        [JsonConstructor]
        private Envelope(object? result, Errors? errors)
        {
            Result = result;
            Errors = errors;
            TimeGenerated = DateTime.Now;
        }

        public static Envelope Ok(object? result = default) => new(result, null);

        public static Envelope Error(Errors errors) => new(default, errors);
    }
}