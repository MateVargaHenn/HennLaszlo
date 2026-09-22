using FluentValidation;

namespace Modules.Artwork.Application.Artworks.Unpublish;

internal sealed class UnpublishArtworkCommandValidator
    : AbstractValidator<UnpublishArtworkCommand>
{
    public UnpublishArtworkCommandValidator()
    {
        RuleFor(command => command.ArtworkId)
            .NotEmpty();
    }
}