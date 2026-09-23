using MediatR;

namespace Modules.Content.Application.ContentPages.GetAdminByKey;

public sealed record GetAdminContentPageByKeyQuery(
    string Key)
    : IRequest<AdminContentPageDetails>;