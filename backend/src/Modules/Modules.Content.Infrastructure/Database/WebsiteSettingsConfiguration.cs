using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Content.Domain;

namespace Modules.Content.Infrastructure.Database;

internal sealed class WebsiteSettingsConfiguration
    : IEntityTypeConfiguration<WebsiteSettings>
{
    public void Configure(
        EntityTypeBuilder<WebsiteSettings> builder)
    {
        builder.ToTable("WebsiteSettings");

        builder.HasKey(settings => settings.Id);

        builder.Property(settings => settings.Id)
            .ValueGeneratedNever();

        builder.Property(settings => settings.ArtistName)
            .HasMaxLength(
                WebsiteSettings.ArtistNameMaxLength)
            .IsRequired();

        builder.Property(settings => settings.ArtistSubtitle)
            .HasMaxLength(
                WebsiteSettings.ArtistSubtitleMaxLength)
            .IsRequired();

        builder.Property(settings => settings.HeroDescription)
            .HasMaxLength(
                WebsiteSettings.HeroDescriptionMaxLength)
            .IsRequired();

        builder.Property(settings => settings.DefaultSeoTitle)
            .HasMaxLength(
                WebsiteSettings.SeoTitleMaxLength)
            .IsRequired();

        builder.Property(settings =>
                settings.DefaultSeoDescription)
            .HasMaxLength(
                WebsiteSettings.SeoDescriptionMaxLength)
            .IsRequired();

        builder.Property(settings => settings.FacebookUrl)
            .HasMaxLength(
                WebsiteSettings.UrlMaxLength);

        builder.Property(settings => settings.InstagramUrl)
            .HasMaxLength(
                WebsiteSettings.UrlMaxLength);

        builder.Property(settings => settings.YoutubeUrl)
            .HasMaxLength(
                WebsiteSettings.UrlMaxLength);

        builder.Property(settings => settings.UpdatedAtUtc)
            .IsRequired();

        builder.HasData(
            new
            {
                Id = WebsiteSettings.SingletonId,
                ArtistName =
                    "Henn László András",
                ArtistSubtitle =
                    "Galyasi Miklós nívódíjas " +
                    "festőművész, grafikus",
                HeroDescription =
                    "Válogatás az alkotó festményeiből, " +
                    "kiállításaiból és több évtizedes " +
                    "művészi munkásságából.",
                DefaultSeoTitle =
                    "Henn László András | " +
                    "Festőművész és grafikus",
                DefaultSeoDescription =
                    "Henn László András Galyasi Miklós " +
                    "nívódíjas festőművész és grafikus " +
                    "hivatalos oldala: művek, kiállítások, " +
                    "meghívók, videók és írások.",
                FacebookUrl = (string?)null,
                InstagramUrl = (string?)null,
                YoutubeUrl = (string?)null,
                UpdatedAtUtc = new DateTime(
                    2026,
                    9,
                    29,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
            });
    }
}
