using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.Create;

internal sealed class CreateArticleCommandHandler(
    IArticleRepository articleRepository,
    IContentUnitOfWork unitOfWork)
    : IRequestHandler<CreateArticleCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateArticleCommand request,
        CancellationToken cancellationToken)
    {
        Domain.Article article =
            Domain.Article.Create(
                request.Slug,
                request.TitleHu,
                request.TitleEn,
                request.SummaryHu,
                request.SummaryEn,
                request.ContentHu,
                request.ContentEn,
                request.DisplayOrder);

        articleRepository.Add(article);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return article.Id;
    }
}