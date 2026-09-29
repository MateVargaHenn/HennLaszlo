using FluentValidation;
using WebsiteSettingsEntity =
    Modules.Content.Domain.WebsiteSettings;

namespace Modules.Content.Application.WebsiteSettings.Update;

internal sealed class UpdateWebsiteSettingsCommandValidator
    : AbstractValidator<UpdateWebsiteSettingsCommand>
{
    public UpdateWebsiteSettingsCommandValidator()
    {
        RuleFor(command => command.ArtistName)
            .NotEmpty()
            .WithMessage(
                "A művész nevének megadása kötelező.")
            .MaximumLength(
                WebsiteSettingsEntity
                    .ArtistNameMaxLength);

        RuleFor(command => command.ArtistSubtitle)
            .NotEmpty()
            .WithMessage(
                "A művész alcímének megadása kötelező.")
            .MaximumLength(
                WebsiteSettingsEntity
                    .ArtistSubtitleMaxLength);

        RuleFor(command => command.HeroDescription)
            .NotEmpty()
            .WithMessage(
                "A kezdőlap leírásának megadása kötelező.")
            .MaximumLength(
                WebsiteSettingsEntity
                    .HeroDescriptionMaxLength);

        RuleFor(command => command.DefaultSeoTitle)
            .NotEmpty()
            .WithMessage(
                "Az alapértelmezett SEO-cím megadása kötelező.")
            .MaximumLength(
                WebsiteSettingsEntity
                    .SeoTitleMaxLength);

        RuleFor(command => command.DefaultSeoDescription)
            .NotEmpty()
            .WithMessage(
                "Az alapértelmezett SEO-leírás megadása kötelező.")
            .MaximumLength(
                WebsiteSettingsEntity
                    .SeoDescriptionMaxLength);

        AddOptionalUrlRule(
            command => command.FacebookUrl);

        AddOptionalUrlRule(
            command => command.InstagramUrl);

        AddOptionalUrlRule(
            command => command.YoutubeUrl);
    }

    private void AddOptionalUrlRule(
        System.Linq.Expressions.Expression<
            Func<
                UpdateWebsiteSettingsCommand,
                string?>> expression)
    {
        RuleFor(expression)
            .MaximumLength(
                WebsiteSettingsEntity.UrlMaxLength)
            .Must(BeValidOptionalHttpUrl)
            .WithMessage(
                "Érvényes http vagy https webcímet adjon meg.");
    }

    private static bool BeValidOptionalHttpUrl(
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        return
            Uri.TryCreate(
                value.Trim(),
                UriKind.Absolute,
                out Uri? uri) &&
            (
                string.Equals(
                    uri.Scheme,
                    Uri.UriSchemeHttp,
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(
                    uri.Scheme,
                    Uri.UriSchemeHttps,
                    StringComparison.OrdinalIgnoreCase));
    }
}
