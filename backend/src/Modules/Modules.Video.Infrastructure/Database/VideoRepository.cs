using Microsoft.EntityFrameworkCore;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Infrastructure.Database;

internal sealed class VideoRepository(
    VideoDbContext dbContext)
    : IVideoRepository
{
    public void Add(Domain.Video video)
    {
        dbContext.Videos.Add(video);
    }

    public void Remove(Domain.Video video)
    {
        dbContext.Videos.Remove(video);
    }

    public Task<Domain.Video?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Videos.SingleOrDefaultAsync(
            video => video.Id == id,
            cancellationToken);
    }

    public async Task<
        IReadOnlyList<Domain.Video>>
        GetOrderedForUpdateAsync(
            CancellationToken cancellationToken =
                default)
    {
        return await dbContext.Videos
            .OrderBy(video =>
                video.DisplayOrder)
            .ThenByDescending(video =>
                video.CreatedAtUtc)
            .ThenBy(video =>
                video.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<Domain.Video?> GetPublishedByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Videos
            .AsNoTracking()
            .SingleOrDefaultAsync(
                video =>
                    video.Id == id &&
                    video.IsPublished,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Domain.Video>>
        GetPublishedAsync(
            CancellationToken cancellationToken = default)
    {
        return await dbContext.Videos
            .AsNoTracking()
            .Where(video => video.IsPublished)
            .OrderBy(video => video.DisplayOrder)
            .ThenByDescending(
                video => video.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Domain.Video>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        return await dbContext.Videos
            .AsNoTracking()
            .OrderBy(video => video.DisplayOrder)
            .ThenByDescending(
                video => video.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}