using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Artwork.Domain;


namespace Modules.Artwork.Infrastructure.Database.Configurations;

internal sealed class ArtworkConfiguration
    : IEntityTypeConfiguration<Domain.Artwork>
{
    public void Configure(EntityTypeBuilder<Domain.Artwork> builder)
    {
        builder.ToTable("Artworks", "artwork");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TitleHu)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.TitleEn)
            .HasMaxLength(250);

        builder.Property(x => x.TechniqueHu)
            .HasMaxLength(250);

        builder.Property(x => x.TechniqueEn)
            .HasMaxLength(250);

        builder.Property(x => x.WidthCm)
            .HasPrecision(8, 2);

        builder.Property(x => x.HeightCm)
            .HasPrecision(8, 2);

        builder.Property(x => x.DescriptionHu)
            .HasMaxLength(4000);

        builder.Property(x => x.DescriptionEn)
            .HasMaxLength(4000);

        builder.Property(x => x.IsPublished)
            .HasDefaultValue(false);

        builder.Property(x => x.IsFeatured)
            .HasDefaultValue(false);

        builder.Property(x => x.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(x => x.IsPublished);

        builder.HasIndex(x => new
        {
            x.IsPublished,
            x.DisplayOrder
        });
    }
}
