namespace DirectoryService.Domain.Shared
{
    public class Error
    {
        private const string SEPARATOR = "||";

        public string Code { get; }

        public string Message { get; }

        public ErrorType Type { get; }

        public string? InvalidField { get; }

        private Error(
            string code,
            string message,
            ErrorType type,
            string? invalidField = null)
        {
            Code = code;
            Message = message;
            Type = type;
            InvalidField = invalidField;
        }

        public static Error Validation(string? code, string message, string? invalidField = default) =>
            new(code ?? "value.is.invalid", message, ErrorType.VALIDATION, invalidField);

        public static Error NotFound(string? code, string message) =>
            new(code ?? "record.not.found", message, ErrorType.NOT_FOUND);

        public static Error Failure(string? code, string message) =>
            new(code ?? "operation.is.failed", message, ErrorType.FAILURE);

        public static Error Conflict(string? code, string message) =>
            new(code ?? "value.is.conflict", message, ErrorType.CONFLICT);

        public static Error Deserialize(string serialized)
        {
            var parts = serialized.Split(SEPARATOR);

            if (parts.Length < 3)
                throw new ArgumentException("Invalid serialized format");

            if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
                throw new ArgumentException("Invalid serialized format");

            return new Error(parts[0], parts[1], type);
        }

        public string Serialize() => string.Join(SEPARATOR, Code, Message, Type);

        public Errors ToErrors() => this;

        public enum ErrorType
        {
            VALIDATION,
            NOT_FOUND,
            CONFLICT,
            FAILURE,
        }
    }
}