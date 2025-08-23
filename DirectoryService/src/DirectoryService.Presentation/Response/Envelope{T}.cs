using DirectoryService.Domain.Shared;

namespace DirectoryService.Presentation.Response
{
    public record Envelope<T>
    {
        public T? Result { get; }

        public Errors? Errors { get; }

        public bool IsError => Errors != null ||
            (Errors != null && Errors.Any());

        public DateTime TimeGenerated { get; }

        private Envelope(T? result, Errors? errors)
        {
            Result = result;
            Errors = errors;
            TimeGenerated = DateTime.Now;
        }

        public static Envelope<T> Ok(T? result = default) => new(result, null);

        public static Envelope<T> Error(Errors errors) => new(default, errors);
    }
}