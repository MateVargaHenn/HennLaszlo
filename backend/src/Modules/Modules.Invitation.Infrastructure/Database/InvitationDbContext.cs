using Microsoft.EntityFrameworkCore;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Infrastructure.Database;

public sealed class InvitationDbContext(
    DbContextOptions<InvitationDbContext> options)
    : DbContext(options),
      IInvitationUnitOfWork
{
    public DbSet<Domain.Invitation> Invitations =>
        Set<Domain.Invitation>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(InvitationDbContext).Assembly);
    }
}