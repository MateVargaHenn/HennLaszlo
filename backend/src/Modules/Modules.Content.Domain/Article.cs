namespace Modules.Content.Domain;

public sealed class Article
{
    private Article()
    {
    }

    private Article(
        string slug,
        string titleHu,
        string? titleEn,
        string? summaryHu,
        string? summaryEn,
        string contentHu,
        string? contentEn,
        int displayOrder)
    {
        Slug = NormalizeSlug(slug);
        TitleHu = titleHu.Trim();
        TitleEn = NormalizeOptionalText(titleEn);
        SummaryHu = NormalizeOptionalText(summaryHu);
        SummaryEn = NormalizeOptionalText(summaryEn);
        ContentHu = contentHu.Trim();
        ContentEn = NormalizeOptionalText(contentEn);
        DisplayOrder = displayOrder;

        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }

    public Guid Id { get; private set; } =
        Guid.NewGuid();

    public string Slug { get; private set; } =
        string.Empty;

    public string TitleHu { get; private set; } =
        string.Empty;

    public string? TitleEn { get; private set; }

    public string? SummaryHu { get; private set; }

    public string? SummaryEn { get; private set; }

    public string ContentHu { get; private set; } =
        string.Empty;

    public string? ContentEn { get; private set; }

    public bool IsPublished { get; private set; }

    public int DisplayOrder { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public static Article Create(
        string slug,
        string titleHu,
        string? titleEn,
        string? summaryHu,
        string? summaryEn,
        string contentHu,
        string? contentEn,
        int displayOrder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            titleHu);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            contentHu);

        ValidateDisplayOrder(displayOrder);

        return new Article(
            slug,
            titleHu,
            titleEn,
            summaryHu,
            summaryEn,
            contentHu,
            contentEn,
            displayOrder);
    }

    public void UpdateDetails(
        string titleHu,
        string? titleEn,
        string? summaryHu,
        string? summaryEn,
        string contentHu,
        string? contentEn,
        int displayOrder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            titleHu);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            contentHu);

        ValidateDisplayOrder(displayOrder);

        TitleHu = titleHu.Trim();
        TitleEn = NormalizeOptionalText(titleEn);
        SummaryHu = NormalizeOptionalText(summaryHu);
        SummaryEn = NormalizeOptionalText(summaryEn);
        ContentHu = contentHu.Trim();
        ContentEn = NormalizeOptionalText(contentEn);
        DisplayOrder = displayOrder;
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

    private static string NormalizeSlug(string slug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        string normalized =
            slug.Trim().ToLowerInvariant();

        bool containsInvalidCharacter =
            normalized.Any(
                character =>
                    !char.IsAsciiLetterOrDigit(character) &&
                    character != '-');

        if (containsInvalidCharacter ||
            normalized.StartsWith('-') ||
            normalized.EndsWith('-') ||
            normalized.Contains("--"))
        {
            throw new ArgumentException(
                "Az URL-azonosító csak kisbetűket, " +
                "számokat és egyszeres kötőjeleket tartalmazhat.",
                nameof(slug));
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

    private static void ValidateDisplayOrder(
        int displayOrder)
    {
        if (displayOrder < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(displayOrder));
        }
    }
}