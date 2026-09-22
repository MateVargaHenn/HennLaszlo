namespace Modules.Invitation.Domain;

public sealed class Invitation
{
    private Invitation()
    {
    }

    private Invitation(
        string titleHu,
        string? titleEn,
        int? year,
        string? altTextHu,
        string? altTextEn,
        int displayOrder)
    {
        TitleHu = titleHu;
        TitleEn = titleEn;
        Year = year;
        AltTextHu = altTextHu;
        AltTextEn = altTextEn;
        DisplayOrder = displayOrder;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string TitleHu { get; private set; } = string.Empty;

    public string? TitleEn { get; private set; }

    public int? Year { get; private set; }

    public string? AltTextHu { get; private set; }

    public string? AltTextEn { get; private set; }

    public Guid? ImageId { get; private set; }

    public bool IsPublished { get; private set; }

    public int DisplayOrder { get; private set; }

    public DateTime CreatedAtUtc { get; private set; } =
        DateTime.UtcNow;

    public static Invitation Create(
        string titleHu,
        string? titleEn,
        int? year,
        string? altTextHu,
        string? altTextEn,
        int displayOrder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(titleHu);

        ValidateYear(year);
        ValidateDisplayOrder(displayOrder);

        return new Invitation(
            titleHu.Trim(),
            NormalizeOptionalText(titleEn),
            year,
            NormalizeOptionalText(altTextHu),
            NormalizeOptionalText(altTextEn),
            displayOrder);
    }

    public void UpdateDetails(
        string titleHu,
        string? titleEn,
        int? year,
        string? altTextHu,
        string? altTextEn,
        int displayOrder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(titleHu);

        ValidateYear(year);
        ValidateDisplayOrder(displayOrder);

        TitleHu = titleHu.Trim();
        TitleEn = NormalizeOptionalText(titleEn);
        Year = year;
        AltTextHu = NormalizeOptionalText(altTextHu);
        AltTextEn = NormalizeOptionalText(altTextEn);
        DisplayOrder = displayOrder;
    }

    public void SetImage(Guid imageId)
    {
        if (imageId == Guid.Empty)
        {
            throw new ArgumentException(
                "A kép azonosítója nem lehet üres.",
                nameof(imageId));
        }

        ImageId = imageId;
    }

    public void RemoveImage()
    {
        if (IsPublished)
        {
            throw new InvalidOperationException(
                "Publikált meghívóról nem távolítható el a kép.");
        }

        ImageId = null;
    }

    public void Publish()
    {
        if (ImageId is null)
        {
            throw new InvalidOperationException(
                "Kép nélküli meghívó nem publikálható.");
        }

        IsPublished = true;
    }

    public void Unpublish()
    {
        IsPublished = false;
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    private static void ValidateYear(int? year)
    {
        if (year is <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(year));
        }

        int maximumYear = DateTime.UtcNow.Year + 1;

        if (year > maximumYear)
        {
            throw new ArgumentOutOfRangeException(
                nameof(year),
                $"Az év nem lehet későbbi, mint {maximumYear}.");
        }
    }

    private static void ValidateDisplayOrder(int displayOrder)
    {
        if (displayOrder < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(displayOrder));
        }
    }
}