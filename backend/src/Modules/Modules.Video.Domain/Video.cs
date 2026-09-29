namespace Modules.Video.Domain;

public sealed class Video
{
    private Video()
    {
    }

    private Video(
        string titleHu,
        string? titleEn,
        int? year,
        string? descriptionHu,
        string? descriptionEn,
        string videoUrl,
        int displayOrder)
    {
        TitleHu = titleHu;
        TitleEn = titleEn;
        Year = year;
        DescriptionHu = descriptionHu;
        DescriptionEn = descriptionEn;
        VideoUrl = videoUrl;
        DisplayOrder = displayOrder;
    }

    public Guid Id { get; private set; } =
        Guid.NewGuid();

    public string TitleHu { get; private set; } =
        string.Empty;

    public string? TitleEn { get; private set; }

    public int? Year { get; private set; }

    public string? DescriptionHu { get; private set; }

    public string? DescriptionEn { get; private set; }

    public string VideoUrl { get; private set; } =
        string.Empty;

    public bool IsPublished { get; private set; }

    public int DisplayOrder { get; private set; }

    public DateTime CreatedAtUtc { get; private set; } =
        DateTime.UtcNow;

    public DateTime? UpdatedAtUtc
    {
        get;
        private set;
    } = DateTime.UtcNow;

    public static Video Create(
        string titleHu,
        string? titleEn,
        int? year,
        string? descriptionHu,
        string? descriptionEn,
        string videoUrl,
        int displayOrder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            titleHu);

        ValidateYear(year);
        ValidateDisplayOrder(displayOrder);

        return new Video(
            titleHu.Trim(),
            NormalizeOptionalText(titleEn),
            year,
            NormalizeOptionalText(descriptionHu),
            NormalizeOptionalText(descriptionEn),
            NormalizeVideoUrl(videoUrl),
            displayOrder);
    }

    public void UpdateDetails(
        string titleHu,
        string? titleEn,
        int? year,
        string? descriptionHu,
        string? descriptionEn,
        string videoUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            titleHu);

        ValidateYear(year);

        TitleHu = titleHu.Trim();
        TitleEn = NormalizeOptionalText(titleEn);
        Year = year;
        DescriptionHu =
            NormalizeOptionalText(descriptionHu);
        DescriptionEn =
            NormalizeOptionalText(descriptionEn);
        VideoUrl = NormalizeVideoUrl(videoUrl);
        MarkAsUpdated();
    }

    public void Publish()
    {
        IsPublished = true;
        MarkAsUpdated();
    }

    public void Unpublish()
    {
        IsPublished = false;
        MarkAsUpdated();
    }

    public void SetDisplayOrder(
        int displayOrder)
    {
        ValidateDisplayOrder(displayOrder);
        DisplayOrder = displayOrder;
        MarkAsUpdated();
    }

    private static string? NormalizeOptionalText(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    private static string NormalizeVideoUrl(
        string videoUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            videoUrl);

        string normalizedUrl = videoUrl.Trim();

        if (
            !Uri.TryCreate(
                normalizedUrl,
                UriKind.Absolute,
                out Uri? uri) ||
            uri.Scheme != Uri.UriSchemeHttps)
        {
            throw new ArgumentException(
                "A videó hivatkozásának érvényes HTTPS URL-nek kell lennie.",
                nameof(videoUrl));
        }

        return uri.AbsoluteUri;
    }

    private static void ValidateYear(
        int? year)
    {
        if (year is <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(year));
        }

        int maximumYear =
            DateTime.UtcNow.Year + 1;

        if (year > maximumYear)
        {
            throw new ArgumentOutOfRangeException(
                nameof(year),
                $"Az év nem lehet későbbi, mint {maximumYear}.");
        }
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

    private void MarkAsUpdated()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}