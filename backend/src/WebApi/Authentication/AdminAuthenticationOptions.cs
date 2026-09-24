namespace WebApi.Authentication;

internal sealed class AdminAuthenticationOptions
{
    internal const string SectionName =
        "AdminAuthentication";

    public string Username { get; init; } =
        string.Empty;

    public string PasswordHash { get; init; } =
        string.Empty;
}