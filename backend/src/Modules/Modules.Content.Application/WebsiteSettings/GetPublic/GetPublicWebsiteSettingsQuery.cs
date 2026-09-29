using MediatR;

namespace Modules.Content.Application.WebsiteSettings.GetPublic;

public sealed record GetPublicWebsiteSettingsQuery
    : IRequest<PublicWebsiteSettings>;
