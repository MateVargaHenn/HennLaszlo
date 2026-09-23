using FluentValidation;
using Modules.Content.Domain;

namespace Modules.Content.Application.ContentPages.Upsert;

internal sealed class UpsertContentPageCommandValidator
    : AbstractValidator<UpsertContentPageCommand>
{
    public UpsertContentPageCommandValidator()
    {
        RuleFor(command => command.Key)
            .NotEmpty()
            .MaximumLength(100)
            .Must(ContentPageKeys.IsSupported)
            .WithMessage(
                "A tartalmi oldal kulcsa nem támogatott.");

        RuleFor(command => command.TitleHu)
            .NotEmpty()
            .WithMessage(
                "A magyar cím megadása kötelező.")
            .MaximumLength(250);

        RuleFor(command => command.TitleEn)
            .MaximumLength(250);

        RuleFor(command => command.ContentHu)
            .NotEmpty()
            .WithMessage(
                "A magyar tartalom megadása kötelező.");
    }
}