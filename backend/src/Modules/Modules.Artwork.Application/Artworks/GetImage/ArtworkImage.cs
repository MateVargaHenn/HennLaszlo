namespace Modules.Artwork.Application.Artworks.GetImage;

public sealed record ArtworkImage(
    Stream Content,
    string ContentType);