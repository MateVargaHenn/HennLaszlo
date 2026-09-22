using FluentValidation;

namespace Modules.Artwork.Application.Artworks.Delete;

internal sealed class DeleteArtworkCommandValidator
    : AbstractValidator<DeleteArtworkCommand>
{
    public DeleteArtworkCommandValidator()
    {
        RuleFor(command => command.ArtworkId)
            .NotEmpty()
            .WithMessage(
                "A mű azonosítója nem lehet üres.");
    }
}