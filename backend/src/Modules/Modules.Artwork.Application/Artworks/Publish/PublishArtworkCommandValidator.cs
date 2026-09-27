using FluentValidation;

namespace Modules.Artwork.Application.Artworks.Publish;

internal sealed class PublishArtworkCommandValidator
    : AbstractValidator<PublishArtworkCommand>
{
    public PublishArtworkCommandValidator()
    {
        RuleFor(command => command.ArtworkId)
            .NotEmpty();
    }
}