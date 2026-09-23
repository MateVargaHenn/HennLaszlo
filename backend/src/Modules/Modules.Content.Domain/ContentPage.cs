namespace Modules.Content.Domain;

public sealed class ContentPage
{
    private ContentPage()
    {
    }

    private ContentPage(
        string key,
        string titleHu,
        string? titleEn,
        string contentHu,
        string? contentEn)
    {
        Key = NormalizeKey(key);
        TitleHu = titleHu.Trim();
        TitleEn = NormalizeOptionalText(titleEn);
        ContentHu = contentHu.Trim();
        ContentEn = NormalizeOptionalText(contentEn);
    }

    public Guid Id { get; private set; } =
        Guid.NewGuid();

    public string Key { get; private set; } =
        string.Empty;

    public string TitleHu { get; private set; } =
        string.Empty;

    public string? TitleEn { get; private set; }

    public string ContentHu { get; private set; } =
        string.Empty;

    public string? ContentEn { get; private set; }

    public bool IsPublished { get; private set; }

    public DateTime CreatedAtUtc { get; private set; } =
        DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; private set; } =
        DateTime.UtcNow;

    public static ContentPage Create(
        string key,
        string titleHu,
        string? titleEn,
        string contentHu,
        string? contentEn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(titleHu);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentHu);

        return new ContentPage(
            key,
            titleHu,
            titleEn,
            contentHu,
            contentEn);
    }

    public void Update(
        string titleHu,
        string? titleEn,
        string contentHu,
        string? contentEn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(titleHu);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentHu);

        TitleHu = titleHu.Trim();
        TitleEn = NormalizeOptionalText(titleEn);
        ContentHu = contentHu.Trim();
        ContentEn = NormalizeOptionalText(contentEn);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Publish()
    {
        IsPublished = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Unpublish()
    {
        IsPublished = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private static string NormalizeKey(
        string key)
    {
        string normalized =
            key.Trim().ToLowerInvariant();

        if (normalized.Length > 100 ||
            normalized.Any(
                character =>
                    !char.IsAsciiLetterOrDigit(character) &&
                    character != '-'))
        {
            throw new ArgumentException(
                "A tartalom kulcsa érvénytelen.",
                nameof(key));
        }

        return normalized;
    }

    private static string? NormalizeOptionalText(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}