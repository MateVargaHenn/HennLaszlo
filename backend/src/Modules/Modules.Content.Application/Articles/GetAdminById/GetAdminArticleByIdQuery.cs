using MediatR;

namespace Modules.Content.Application.Articles.GetAdminById;

public sealed record GetAdminArticleByIdQuery(
    Guid ArticleId)
    : IRequest<AdminArticleDetails>;