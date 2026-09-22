using FluentValidation;
using Modules.Artwork.Application.Artworks.Common;

namespace Modules.Artwork.Application.Artworks.Update;

internal sealed class UpdateArtworkCommandValidator
    : ArtworkDetailsCommandValidator<UpdateArtworkCommand>
{
    public UpdateArtworkCommandValidator()
    {
        RuleFor(command => command.ArtworkId)
            .NotEmpty();

        RuleFor(command => command.DescriptionHu)
            .MaximumLength(5000);

        RuleFor(command => command.DescriptionEn)
            .MaximumLength(5000);

        RuleFor(command => command.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}