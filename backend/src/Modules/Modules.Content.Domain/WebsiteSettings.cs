using System.Net.Mail;

namespace Modules.Content.Domain;

public sealed class WebsiteSettings
{
    public const int ArtistNameMaxLength = 200;
    public const int ArtistSubtitleMaxLength = 300;
    public const int HeroDescriptionMaxLength = 1000;
    public const int SeoTitleMaxLength = 200;
    public const int SeoDescriptionMaxLength = 500;
    public const int UrlMaxLength = 2048;
    public const int EmailAddressMaxLength = 254;

    public static readonly Guid SingletonId =
        Guid.Parse(
            "3d932c73-5380-4fb9-ae2f-394011659d73");

    private WebsiteSettings()
    {
    }

    private WebsiteSettings(
        string artistName,
        string artistSubtitle,
        string heroDescription,
        string defaultSeoTitle,
        string defaultSeoDescription,
        string? facebookUrl,
        string? instagramUrl,
        string? youtubeUrl,
        string? emailAddress)
    {
        Id = SingletonId;

        SetValues(
            artistName,
            artistSubtitle,
            heroDescription,
            defaultSeoTitle,
            defaultSeoDescription,
            facebookUrl,
            instagramUrl,
            youtubeUrl,
            emailAddress);
    }

    public Guid Id { get; private set; } =
        SingletonId;

    public string ArtistName { get; private set; } =
        string.Empty;

    public string ArtistSubtitle { get; private set; } =
        string.Empty;

    public string HeroDescription { get; private set; } =
        string.Empty;

    public string DefaultSeoTitle { get; private set; } =
        string.Empty;

    public string DefaultSeoDescription { get; private set; } =
        string.Empty;

    public string? FacebookUrl { get; private set; }

    public string? InstagramUrl { get; private set; }

    public string? YoutubeUrl { get; private set; }

    public string? EmailAddress { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; } =
        DateTime.UtcNow;

    public static WebsiteSettings Create(
        string artistName,
        string artistSubtitle,
        string heroDescription,
        string defaultSeoTitle,
        string defaultSeoDescription,
        string? facebookUrl,
        string? instagramUrl,
        string? youtubeUrl,
        string? emailAddress)
    {
        return new WebsiteSettings(
            artistName,
            artistSubtitle,
            heroDescription,
            defaultSeoTitle,
            defaultSeoDescription,
            facebookUrl,
            instagramUrl,
            youtubeUrl,
            emailAddress);
    }

    public void Update(
        string artistName,
        string artistSubtitle,
        string heroDescription,
        string defaultSeoTitle,
        string defaultSeoDescription,
        string? facebookUrl,
        string? instagramUrl,
        string? youtubeUrl,
        string? emailAddress)
    {
        SetValues(
            artistName,
            artistSubtitle,
            heroDescription,
            defaultSeoTitle,
            defaultSeoDescription,
            facebookUrl,
            instagramUrl,
            youtubeUrl,
            emailAddress);

        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetValues(
        string artistName,
        string artistSubtitle,
        string heroDescription,
        string defaultSeoTitle,
        string defaultSeoDescription,
        string? facebookUrl,
        string? instagramUrl,
        string? youtubeUrl,
        string? emailAddress)
    {
        ArtistName = NormalizeRequired(
            artistName,
            ArtistNameMaxLength,
            nameof(artistName));

        ArtistSubtitle = NormalizeRequired(
            artistSubtitle,
            ArtistSubtitleMaxLength,
            nameof(artistSubtitle));

        HeroDescription = NormalizeRequired(
            heroDescription,
            HeroDescriptionMaxLength,
            nameof(heroDescription));

        DefaultSeoTitle = NormalizeRequired(
            defaultSeoTitle,
            SeoTitleMaxLength,
            nameof(defaultSeoTitle));

        DefaultSeoDescription = NormalizeRequired(
            defaultSeoDescription,
            SeoDescriptionMaxLength,
            nameof(defaultSeoDescription));

        FacebookUrl = NormalizeOptionalUrl(
            facebookUrl,
            nameof(facebookUrl));

        InstagramUrl = NormalizeOptionalUrl(
            instagramUrl,
            nameof(instagramUrl));

        YoutubeUrl = NormalizeOptionalUrl(
            youtubeUrl,
            nameof(youtubeUrl));
        
        EmailAddress = NormalizeOptionalEmail(
            emailAddress,
            nameof(emailAddress));
    }

    private static string? NormalizeOptionalEmail(
    string? value,
    string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string normalized = value.Trim();

        if (
            normalized.Length >
                EmailAddressMaxLength ||
            !MailAddress.TryCreate(
                normalized,
                out MailAddress? address) ||
            !string.Equals(
                address.Address,
                normalized,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "A megadott e-mail-cím érvénytelen.",
                parameterName);
        }

        return normalized;
    }

    private static string NormalizeRequired(
        string value,
        int maximumLength,
        string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            value,
            parameterName);

        string normalized = value.Trim();

        if (normalized.Length > maximumLength)
        {
            throw new ArgumentException(
                "A megadott szöveg túl hosszú.",
                parameterName);
        }

        return normalized;
    }

    private static string? NormalizeOptionalUrl(
        string? value,
        string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string normalized = value.Trim();

        if (
            normalized.Length > UrlMaxLength ||
            !Uri.TryCreate(
                normalized,
                UriKind.Absolute,
                out Uri? uri) ||
            !IsHttpScheme(uri.Scheme))
        {
            throw new ArgumentException(
                "A megadott webcím érvénytelen.",
                parameterName);
        }

        return normalized;
    }

    private static bool IsHttpScheme(
        string scheme)
    {
        return
            string.Equals(
                scheme,
                Uri.UriSchemeHttp,
                StringComparison.OrdinalIgnoreCase) ||
            string.Equals(
                scheme,
                Uri.UriSchemeHttps,
                StringComparison.OrdinalIgnoreCase);
    }
}
