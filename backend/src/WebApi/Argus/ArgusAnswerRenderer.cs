using MediatR;
using Modules.Argus.Application.Abstractions;
using Modules.Content.Application.WebsiteSettings.GetPublic;

namespace WebApi.Argus;

internal sealed class ArgusAnswerRenderer(ISender sender)
    : IArgusAnswerRenderer
{
    public async Task<string> RenderAsync(
        string answer,
        CancellationToken cancellationToken = default)
    {
        const string emailPlaceholder = "{{email}}";

        if (!answer.Contains(
                emailPlaceholder,
                StringComparison.Ordinal))
        {
            return answer;
        }

        PublicWebsiteSettings settings = await sender.Send(
            new GetPublicWebsiteSettingsQuery(),
            cancellationToken);

        if (string.IsNullOrWhiteSpace(settings.EmailAddress))
        {
            return "Kapcsolatfelvételhez tekintse meg " +
                "a Kapcsolat oldalt.";
        }

        return answer.Replace(
            emailPlaceholder,
            settings.EmailAddress.Trim(),
            StringComparison.Ordinal);
    }
}