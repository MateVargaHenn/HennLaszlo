using Microsoft.EntityFrameworkCore;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Infrastructure.Database;

internal sealed class ArticleRepository(
    ContentDbContext dbContext)
    : IArticleRepository
{
    public void Add(Domain.Article article)
    {
        dbContext.Articles.Add(article);
    }

    public void Remove(Domain.Article article)
    {
        dbContext.Articles.Remove(article);
    }

    public Task<Domain.Article?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Articles
            .SingleOrDefaultAsync(
                article => article.Id == id,
                cancellationToken);
    }

    public Task<Domain.Article?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        string normalizedSlug =
            NormalizeSlug(slug);

        return dbContext.Articles
            .SingleOrDefaultAsync(
                article =>
                    article.Slug == normalizedSlug,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Domain.Article>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        return await dbContext.Articles
            .AsNoTracking()
            .OrderBy(article => article.DisplayOrder)
            .ThenBy(article => article.TitleHu)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Domain.Article>>
        GetPublishedAsync(
            CancellationToken cancellationToken = default)
    {
        return await dbContext.Articles
            .AsNoTracking()
            .Where(article => article.IsPublished)
            .OrderBy(article => article.DisplayOrder)
            .ThenBy(article => article.TitleHu)
            .ToListAsync(cancellationToken);
    }

    public Task<Domain.Article?>
        GetPublishedBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
    {
        string normalizedSlug =
            NormalizeSlug(slug);

        return dbContext.Articles
            .AsNoTracking()
            .SingleOrDefaultAsync(
                article =>
                    article.Slug == normalizedSlug &&
                    article.IsPublished,
                cancellationToken);
    }

    private static string NormalizeSlug(string slug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        return slug.Trim().ToLowerInvariant();
    }
}