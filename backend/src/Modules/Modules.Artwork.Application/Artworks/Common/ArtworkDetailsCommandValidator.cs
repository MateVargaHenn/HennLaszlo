using FluentValidation;

namespace Modules.Artwork.Application.Artworks.Common;

internal abstract class ArtworkDetailsCommandValidator<TCommand>
    : AbstractValidator<TCommand>
    where TCommand : IArtworkDetailsCommand
{
    protected ArtworkDetailsCommandValidator()
    {
        RuleFor(command => command.TitleHu)
            .NotEmpty()
            .WithMessage(
                "A magyar cím megadása kötelező.")
            .MaximumLength(250);

        RuleFor(command => command.TitleEn)
            .MaximumLength(250);

        RuleFor(command => command.TechniqueHu)
            .MaximumLength(250);

        RuleFor(command => command.TechniqueEn)
            .MaximumLength(250);

        RuleFor(command => command.Year)
            .GreaterThan(0)
            .LessThanOrEqualTo(
                DateTime.UtcNow.Year + 1)
            .When(command => command.Year.HasValue);

        RuleFor(command => command.WidthCm)
            .GreaterThan(0)
            .When(command => command.WidthCm.HasValue);

        RuleFor(command => command.HeightCm)
            .GreaterThan(0)
            .When(command => command.HeightCm.HasValue);
    }
}