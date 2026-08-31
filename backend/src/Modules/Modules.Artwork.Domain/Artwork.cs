namespace Modules.Artwork.Domain;

public sealed class Artwork
{
    private Artwork()
    {
    }

    private Artwork(
        string titleHu,
        string? titleEn,
        int? year,
        string? techniqueHu,
        string? techniqueEn,
        decimal? widthCm,
        decimal? heightCm)
    {
        TitleHu = titleHu;
        TitleEn = titleEn;
        Year = year;
        TechniqueHu = techniqueHu;
        TechniqueEn = techniqueEn;
        WidthCm = widthCm;
        HeightCm = heightCm;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string TitleHu { get; private set; } = string.Empty;

    public string? TitleEn { get; private set; }

    public int? Year { get; private set; }

    public string? TechniqueHu { get; private set; }

    public string? TechniqueEn { get; private set; }

    public decimal? WidthCm { get; private set; }

    public decimal? HeightCm { get; private set; }

    public string? DescriptionHu { get; private set; }

    public string? DescriptionEn { get; private set; }

    public Guid? ImageId { get; private set; }

    public bool IsPublished { get; private set; }

    public bool IsFeatured { get; private set; }

    public int DisplayOrder { get; private set; }

    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public static Artwork Create(
        string titleHu,
        string? titleEn,
        int? year,
        string? techniqueHu,
        string? techniqueEn,
        decimal? widthCm,
        decimal? heightCm)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(titleHu);

        if (year is <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(year));
        }

        if (widthCm is <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(widthCm));
        }

        if (heightCm is <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(heightCm));
        }

        return new Artwork(
            titleHu.Trim(),
            NormalizeOptionalText(titleEn),
            year,
            NormalizeOptionalText(techniqueHu),
            NormalizeOptionalText(techniqueEn),
            widthCm,
            heightCm);
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}