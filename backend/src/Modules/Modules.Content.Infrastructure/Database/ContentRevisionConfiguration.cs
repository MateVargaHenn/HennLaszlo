using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Content.Domain;

namespace Modules.Content.Infrastructure.Database;

internal sealed class ContentRevisionConfiguration
    : IEntityTypeConfiguration<ContentRevision>
{
    public void Configure(
        EntityTypeBuilder<ContentRevision> builder)
    {
        builder.ToTable("ContentRevisions");

        builder.HasKey(revision =>
            revision.Id);

        builder.Property(revision =>
                revision.Id)
            .ValueGeneratedNever();

        builder.Property(revision =>
                revision.TargetType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(revision =>
                revision.TargetId)
            .IsRequired();

        builder.Property(revision =>
                revision.TitleHu)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(revision =>
                revision.TitleEn)
            .HasMaxLength(250);

        builder.Property(revision =>
                revision.SummaryHu)
            .HasMaxLength(1000);

        builder.Property(revision =>
                revision.SummaryEn)
            .HasMaxLength(1000);

        builder.Property(revision =>
                revision.ContentHu)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(revision =>
                revision.ContentEn)
            .HasColumnType("text");

        builder.Property(revision =>
                revision.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(revision =>
            new
            {
                revision.TargetType,
                revision.TargetId,
                revision.CreatedAtUtc,
            });
    }
}