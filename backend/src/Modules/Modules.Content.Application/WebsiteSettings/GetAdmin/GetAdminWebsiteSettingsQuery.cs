using MediatR;

namespace Modules.Content.Application.WebsiteSettings.GetAdmin;

public sealed record GetAdminWebsiteSettingsQuery
    : IRequest<AdminWebsiteSettingsDetails>;
