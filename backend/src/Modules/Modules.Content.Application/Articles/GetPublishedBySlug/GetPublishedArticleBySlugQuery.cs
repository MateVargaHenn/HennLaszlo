using MediatR;

namespace Modules.Content.Application.Articles.GetPublishedBySlug;

public sealed record GetPublishedArticleBySlugQuery(
    string Slug)
    : IRequest<PublishedArticleDetails>;