namespace Modules.Artwork.Domain;

public sealed class Artwork
{
    private Artwork()
    {
    }

    private Artwork(
            string titleHu,
            string titleEn,
            int? year,
            string techniqueHu,
            string techniqueEn,
            decimal? widthCm,
            decimal? heightCm,
            string? descriptionHu,
            string? descriptionEn,
            bool isFeatured,
            int displayOrder)
    {
        TitleHu = titleHu;
        TitleEn = titleEn;
        Year = year;
        TechniqueHu = techniqueHu;
        TechniqueEn = techniqueEn;
        WidthCm = widthCm;
        HeightCm = heightCm;
        DescriptionHu = descriptionHu;
        DescriptionEn = descriptionEn;
        IsFeatured = isFeatured;
        DisplayOrder = displayOrder;
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
            string titleEn,
            int? year,
            string techniqueHu,
            string techniqueEn,
            decimal? widthCm,
            decimal? heightCm,
            string? descriptionHu,
            string? descriptionEn,
            bool isFeatured,
            int displayOrder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(titleHu);

		ValidateYear(year);
		ValidateDimensions(widthCm, heightCm);

        return new Artwork(
            titleHu.Trim(),
            NormalizeOptionalText(titleEn)!,
            year,
            NormalizeOptionalText(techniqueHu)!,
            NormalizeOptionalText(techniqueEn)!,
            widthCm,
            heightCm,
            NormalizeOptionalText(descriptionHu)!,
            NormalizeOptionalText(descriptionEn)!,
            isFeatured,
            displayOrder)
        {
            DescriptionHu = NormalizeOptionalText(descriptionHu),
            DescriptionEn = NormalizeOptionalText(descriptionEn),
            IsFeatured = isFeatured,
            DisplayOrder = displayOrder
        };
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

	public void UpdateDetails(
    string titleHu,
    string? titleEn,
    int? year,
    string? techniqueHu,
    string? techniqueEn,
    decimal? widthCm,
    decimal? heightCm,
    string? descriptionHu,
    string? descriptionEn,
	int displayOrder)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(titleHu);

		ValidateYear(year);
		ValidateDimensions(widthCm, heightCm);

		TitleHu = titleHu.Trim();
		TitleEn = NormalizeOptionalText(titleEn);
		Year = year;
		TechniqueHu = NormalizeOptionalText(techniqueHu);
		TechniqueEn = NormalizeOptionalText(techniqueEn);
		WidthCm = widthCm;
		HeightCm = heightCm;
		DescriptionHu = NormalizeOptionalText(descriptionHu);
		DescriptionEn = NormalizeOptionalText(descriptionEn);
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
				"Publikált műről nem távolítható el a kép.");
		}

		ImageId = null;
	}

	public void Publish()
	{
		if (ImageId is null)
		{
			throw new InvalidOperationException(
				"Kép nélküli mű nem publikálható.");
		}

		IsPublished = true;
	}

	public void Unpublish()
	{
		IsPublished = false;
		IsFeatured = false;
	}

	public void SetFeatured(bool isFeatured)
	{
		if (isFeatured && !IsPublished)
		{
			throw new InvalidOperationException(
				"Csak publikált mű lehet kiemelt.");
		}

		IsFeatured = isFeatured;
	}

	public void SetDisplayOrder(int displayOrder)
	{
		if (displayOrder < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(displayOrder));
		}

		DisplayOrder = displayOrder;
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

	private static void ValidateDimensions(
		decimal? widthCm,
		decimal? heightCm)
	{
		if (widthCm is <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(widthCm));
		}

		if (heightCm is <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(heightCm));
		}
	}
}