namespace Modules.Content.Application.Abstractions;

public interface IContentUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}