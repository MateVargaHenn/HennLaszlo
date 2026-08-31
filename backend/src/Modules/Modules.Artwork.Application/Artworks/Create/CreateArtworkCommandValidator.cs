using FluentValidation;

namespace Modules.Artwork.Application.Artworks.Create;

internal sealed class CreateArtworkCommandValidator
    : AbstractValidator<CreateArtworkCommand>
{
    public CreateArtworkCommandValidator()
    {
        RuleFor(x => x.TitleHu)
            .NotEmpty()
            .WithMessage("A magyar cím megadása kötelező.")
            .MaximumLength(250);

        RuleFor(x => x.TitleEn)
            .MaximumLength(250);

        RuleFor(x => x.TechniqueHu)
            .MaximumLength(250);

        RuleFor(x => x.TechniqueEn)
            .MaximumLength(250);

        RuleFor(x => x.Year)
            .GreaterThan(0)
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1)
            .When(x => x.Year.HasValue);

        RuleFor(x => x.WidthCm)
            .GreaterThan(0)
            .When(x => x.WidthCm.HasValue);

        RuleFor(x => x.HeightCm)
            .GreaterThan(0)
            .When(x => x.HeightCm.HasValue);
    }
}