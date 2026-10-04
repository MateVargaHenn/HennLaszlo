using Microsoft.EntityFrameworkCore;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Infrastructure.Database;

internal sealed class ContentPageRepository(
    ContentDbContext dbContext)
    : IContentPageRepository
{
    public void Add(
        Domain.ContentPage contentPage)
    {
        dbContext.ContentPages.Add(contentPage);
    }

    public async Task<Domain.ContentPage?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ContentPages
            .SingleOrDefaultAsync(
                contentPage => contentPage.Id == id,
                cancellationToken);
    }

    public async Task<Domain.ContentPage?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ContentPages
            .SingleOrDefaultAsync(
                contentPage => contentPage.Key == key,
                cancellationToken);
    }

    public async Task<Domain.ContentPage?>
        GetPublishedByKeyAsync(
            string key,
            CancellationToken cancellationToken = default)
    {
        return await dbContext.ContentPages
            .AsNoTracking()
            .SingleOrDefaultAsync(
                contentPage =>
                    contentPage.Key == key &&
                    contentPage.IsPublished,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Domain.ContentPage>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        return await dbContext.ContentPages
            .AsNoTracking()
            .OrderBy(contentPage =>
                contentPage.Key)
            .ToListAsync(cancellationToken);
    }
    public Task<bool> HasPublishedFileReferenceAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        // Include the closing HTML attribute quote to avoid matching ID prefixes.
        string doubleQuotedPath = filePath + "\"";
        string singleQuotedPath = filePath + "'";

        return dbContext.ContentPages.AnyAsync(
            contentPage => contentPage.IsPublished && (
                contentPage.ContentHu.Contains(doubleQuotedPath) ||
                contentPage.ContentHu.Contains(singleQuotedPath) ||
                (contentPage.ContentEn != null && (
                    contentPage.ContentEn.Contains(doubleQuotedPath) ||
                    contentPage.ContentEn.Contains(singleQuotedPath)))),
            cancellationToken);
    }

}