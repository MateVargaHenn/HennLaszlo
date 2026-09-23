using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Content.Infrastructure.Database;

internal sealed class ArticleConfiguration
    : IEntityTypeConfiguration<Domain.Article>
{
    public void Configure(
        EntityTypeBuilder<Domain.Article> builder)
    {
        builder.ToTable(
            "Articles",
            "content");

        builder.HasKey(article => article.Id);

        builder.Property(article => article.Slug)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(article => article.Slug)
            .IsUnique();

        builder.Property(article => article.TitleHu)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(article => article.TitleEn)
            .HasMaxLength(250);

        builder.Property(article => article.SummaryHu)
            .HasMaxLength(1000);

        builder.Property(article => article.SummaryEn)
            .HasMaxLength(1000);

        builder.Property(article => article.ContentHu)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(article => article.ContentEn)
            .HasColumnType("text");

        builder.Property(article => article.IsPublished)
            .IsRequired();

        builder.Property(article => article.DisplayOrder)
            .IsRequired();

        builder.Property(article => article.CreatedAtUtc)
            .IsRequired();

        builder.Property(article => article.UpdatedAtUtc)
            .IsRequired();

        builder.HasIndex(
            article => new
            {
                article.IsPublished,
                article.DisplayOrder,
            });
    }
}