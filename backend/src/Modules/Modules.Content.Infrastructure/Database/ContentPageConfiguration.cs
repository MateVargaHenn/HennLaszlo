using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Content.Domain;

namespace Modules.Content.Infrastructure.Database;

internal sealed class ContentPageConfiguration
    : IEntityTypeConfiguration<ContentPage>
{
    public void Configure(
        EntityTypeBuilder<ContentPage> builder)
    {
        builder.ToTable("ContentPages");

        builder.HasKey(contentPage =>
            contentPage.Id);

        builder.Property(contentPage =>
                contentPage.Id)
            .ValueGeneratedNever();

        builder.Property(contentPage =>
                contentPage.Key)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(contentPage =>
                contentPage.Key)
            .IsUnique();

        builder.Property(contentPage =>
                contentPage.TitleHu)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(contentPage =>
                contentPage.TitleEn)
            .HasMaxLength(250);

        builder.Property(contentPage =>
                contentPage.ContentHu)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(contentPage =>
                contentPage.ContentEn)
            .HasColumnType("text");

        builder.Property(contentPage =>
                contentPage.IsPublished)
            .IsRequired();

        builder.Property(contentPage =>
                contentPage.CreatedAtUtc)
            .IsRequired();

        builder.Property(contentPage =>
                contentPage.UpdatedAtUtc)
            .IsRequired();
    }
}