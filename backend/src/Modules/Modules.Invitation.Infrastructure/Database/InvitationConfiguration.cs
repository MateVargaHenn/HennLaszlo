using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Invitation.Infrastructure.Database;

internal sealed class InvitationConfiguration
    : IEntityTypeConfiguration<Domain.Invitation>
{
    public void Configure(
        EntityTypeBuilder<Domain.Invitation> builder)
    {
        builder.ToTable(
            "Invitations",
            "invitation");

        builder.HasKey(invitation => invitation.Id);

        builder.Property(invitation => invitation.TitleHu)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(invitation => invitation.TitleEn)
            .HasMaxLength(250);

        builder.Property(invitation => invitation.AltTextHu)
            .HasMaxLength(500);

        builder.Property(invitation => invitation.AltTextEn)
            .HasMaxLength(500);

        builder.Property(invitation => invitation.IsPublished)
            .IsRequired();

        builder.Property(invitation => invitation.DisplayOrder)
            .IsRequired();

        builder.Property(invitation => invitation.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(invitation => new
        {
            invitation.IsPublished,
            invitation.DisplayOrder,
        });
    }
}