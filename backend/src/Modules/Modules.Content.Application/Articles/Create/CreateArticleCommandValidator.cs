using FluentValidation;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.Create;

internal sealed class CreateArticleCommandValidator
    : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator(
        IArticleRepository articleRepository)
    {
        RuleFor(command => command.Slug)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(
                "Az URL-azonosító megadása kötelező.")
            .MaximumLength(200)
            .Matches(
                "^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage(
                "Az URL-azonosító csak kisbetűket, " +
                "számokat és kötőjeleket tartalmazhat.")
            .MustAsync(
                async (slug, cancellationToken) =>
                    await articleRepository.GetBySlugAsync(
                        slug,
                        cancellationToken) is null)
            .WithMessage(
                "Ez az URL-azonosító már foglalt.");

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