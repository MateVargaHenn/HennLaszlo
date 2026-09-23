using MediatR;

namespace Modules.Content.Application.ContentPages.GetAdminList;

public sealed record GetAdminContentPagesQuery
    : IRequest<
        IReadOnlyList<AdminContentPageListItem>>;