namespace DirectoryService.Domain.Shared;

public static class Constants
{
    public const int MIN_DEPARTMENT_NAME_LENGTH = 3;
    public const int MAX_DEPARTMENT_NAME_LENGTH = 150;

    public const int MIN_LOCATION_NAME_LENGTH = 3;
    public const int MAX_LOCATION_NAME_LENGTH = 120;

    public const int MIN_POSITION_NAME_LENGTH = 3;
    public const int MAX_POSITION_NAME_LENGTH = 100;

    public const int MIN_IDENTIFIER_LENGTH = 3;
    public const int MAX_IDENTIFIER_LENGTH = 150;

    public const int MAX_POSITION_DESCRIPTION_LENGTH = 1000;

    public const string IDENTIFIER_REGEX = "^[a-z]+$";

    public const string TIME_ZONE_REGEX =
        "^(?:[A-Za-z0-9]+(?:[-_A-Za-z0-9+.]*[A-Za-z0-9])?)(?:/(?:[A-Za-z0-9]+(?:[-_A-Za-z0-9+.]*[A-Za-z0-9])?))*$";

}