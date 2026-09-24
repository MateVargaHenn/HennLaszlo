namespace Modules.Content.Domain;

public static class ContentPageKeys
{
    public const string About =
        "about";

    public const string Exhibitions =
        "exhibitions";

    public const string MembershipsAndAwards =
        "memberships-and-awards";

    public const string Writings =
        "writings";

    public static bool IsSupported(
        string key)
    {
        return key is
            About or
            Exhibitions or
            MembershipsAndAwards or
            Writings;
    }
}