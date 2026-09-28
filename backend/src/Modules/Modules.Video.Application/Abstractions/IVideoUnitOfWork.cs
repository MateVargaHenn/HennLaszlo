namespace Modules.Video.Application.Abstractions;

public interface IVideoUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}