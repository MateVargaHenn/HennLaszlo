using FluentValidation;

namespace Modules.Content.Application.Articles.Update;

internal sealed class UpdateArticleCommandValidator
    : AbstractValidator<UpdateArticleCommand>
{
    public UpdateArticleCommandValidator()
    {
        RuleFor(command => command.ArticleId)
            .NotEmpty();

        RuleFor(command => command.TitleHu)
            .NotEmpty()
            .WithMessage(
                "A magyar cím megadása kötelező.")
            .MaximumLength(250);

        RuleFor(command => command.TitleEn)
            .MaximumLength(250);

        RuleFor(command => command.SummaryHu)
            .MaximumLength(1000);

        RuleFor(command => command.SummaryEn)
            .MaximumLength(1000);

        RuleFor(command => command.ContentHu)
            .NotEmpty()
            .WithMessage(
                "A magyar tartalom megadása kötelező.");

        RuleFor(command => command.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}