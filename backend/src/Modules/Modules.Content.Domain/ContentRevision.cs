namespace Modules.Content.Domain;

public sealed class ContentRevision
{
    private ContentRevision()
    {
    }

    private ContentRevision(
        ContentRevisionTargetType targetType,
        Guid targetId,
        string titleHu,
        string? titleEn,
        string? summaryHu,
        string? summaryEn,
        string contentHu,
        string? contentEn)
    {
        TargetType = targetType;
        TargetId = targetId;
        TitleHu = titleHu;
        TitleEn = titleEn;
        SummaryHu = summaryHu;
        SummaryEn = summaryEn;
        ContentHu = contentHu;
        ContentEn = contentEn;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; } =
        Guid.NewGuid();

    public ContentRevisionTargetType TargetType
    {
        get;
        private set;
    }

    public Guid TargetId { get; private set; }

    public string TitleHu { get; private set; } =
        string.Empty;

    public string? TitleEn { get; private set; }

    public string? SummaryHu { get; private set; }

    public string? SummaryEn { get; private set; }

    public string ContentHu { get; private set; } =
        string.Empty;

    public string? ContentEn { get; private set; }

    public DateTime CreatedAtUtc
    {
        get;
        private set;
    }

    public static ContentRevision Capture(
        Article article)
    {
        ArgumentNullException.ThrowIfNull(article);

        return new ContentRevision(
            ContentRevisionTargetType.Article,
            article.Id,
            article.TitleHu,
            article.TitleEn,
            article.SummaryHu,
            article.SummaryEn,
            article.ContentHu,
            article.ContentEn);
    }

    public static ContentRevision Capture(
        ContentPage contentPage)
    {
        ArgumentNullException.ThrowIfNull(
            contentPage);

        return new ContentRevision(
            ContentRevisionTargetType.ContentPage,
            contentPage.Id,
            contentPage.TitleHu,
            contentPage.TitleEn,
            null,
            null,
            contentPage.ContentHu,
            contentPage.ContentEn);
    }
}