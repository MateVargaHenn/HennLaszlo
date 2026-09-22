namespace Modules.Artwork.Application.Artworks.Common;

public interface IArtworkDetailsCommand
{
    string TitleHu { get; }

    string? TitleEn { get; }

    int? Year { get; }

    string? TechniqueHu { get; }

    string? TechniqueEn { get; }

    decimal? WidthCm { get; }

    decimal? HeightCm { get; }
}