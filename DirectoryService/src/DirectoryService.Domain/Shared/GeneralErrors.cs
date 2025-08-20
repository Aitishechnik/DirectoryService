namespace DirectoryService.Domain.Shared
{
    public static class GeneralErrors
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var lable = name ?? "value";
            return Error.Validation("value.is.invalid", $"{lable} is invalid");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? string.Empty : $" for id '{id}'";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var lable = name ?? " ";
            return Error.Validation("length.is.invalid", $"Invalid {lable} length");
        }

        public static Error AlreadyDeleted(Guid id)
        {
            return Error.Validation("record.already.deleted", $"Record with id {id} is already deleted");
        }

        public static Error AlreadyExist(string parameter, string parameterName)
        {
            return Error.Validation("record.already.exist", $"Record with {parameterName} {parameter} is already exist");
        }
    }
}