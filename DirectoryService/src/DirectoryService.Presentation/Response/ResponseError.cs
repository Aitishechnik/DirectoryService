namespace DirectoryService.Presentation.Response
{
    public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);
}