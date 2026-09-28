using FluentValidation;
using Modules.Video.Application.Videos;

namespace Modules.Video.Application.Videos.Update;

internal sealed class UpdateVideoCommandValidator
    : AbstractValidator<UpdateVideoCommand>
{
    public UpdateVideoCommandValidator()
    {
        RuleFor(command => command.VideoId)
            .NotEmpty();

        RuleFor(command => command.TitleHu)
            .NotEmpty()
            .WithMessage(
                "A magyar cím megadása kötelező.")
            .MaximumLength(250);

        RuleFor(command => command.TitleEn)
            .MaximumLength(250);

        RuleFor(command => command.Year)
            .GreaterThan(0)
            .LessThanOrEqualTo(
                DateTime.UtcNow.Year + 1)
            .When(command => command.Year.HasValue);

        RuleFor(command => command.DescriptionHu)
            .MaximumLength(4000);

        RuleFor(command => command.DescriptionEn)
            .MaximumLength(4000);

        RuleFor(command => command.VideoUrl)
            .NotEmpty()
            .MaximumLength(2048)
            .Must(VideoUrlValidator.IsValid)
            .WithMessage(
                "A videó hivatkozásának érvényes HTTPS URL-nek kell lennie.");

        RuleFor(command => command.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}