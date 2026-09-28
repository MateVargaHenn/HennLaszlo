using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Video.Infrastructure.Database;

internal sealed class VideoConfiguration
    : IEntityTypeConfiguration<Domain.Video>
{
    public void Configure(
        EntityTypeBuilder<Domain.Video> builder)
    {
        builder.ToTable(
            "Videos",
            "video");

        builder.HasKey(video => video.Id);

        builder.Property(video => video.TitleHu)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(video => video.TitleEn)
            .HasMaxLength(250);

        builder.Property(video => video.DescriptionHu)
            .HasMaxLength(4000);

        builder.Property(video => video.DescriptionEn)
            .HasMaxLength(4000);

        builder.Property(video => video.VideoUrl)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(video => video.IsPublished)
            .IsRequired();

        builder.Property(video => video.DisplayOrder)
            .IsRequired();

        builder.Property(video => video.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(video => new
        {
            video.IsPublished,
            video.DisplayOrder,
        });
    }
}