using FluentValidation;

namespace Modules.Artwork.Application.Artworks.AttachImage;

internal sealed class AttachArtworkImageCommandValidator
    : AbstractValidator<AttachArtworkImageCommand>
{
    public AttachArtworkImageCommandValidator()
    {
        RuleFor(x => x.ArtworkId)
            .NotEmpty();

        RuleFor(x => x.FileId)
            .NotEmpty();
    }
}