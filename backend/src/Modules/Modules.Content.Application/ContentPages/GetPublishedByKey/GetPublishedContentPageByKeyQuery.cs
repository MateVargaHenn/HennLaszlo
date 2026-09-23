using MediatR;

namespace Modules.Content.Application.ContentPages.GetPublishedByKey;

public sealed record GetPublishedContentPageByKeyQuery(
    string Key)
    : IRequest<PublishedContentPageDetails>;