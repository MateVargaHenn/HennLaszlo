using System.Globalization;
using MediatR;
using Modules.Argus.Application.Abstractions;
using Modules.Content.Application.WebsiteSettings.GetPublic;
using Modules.Invitation.Application.Invitations.GetAll;

namespace WebApi.Argus;

internal sealed class ArgusAnswerRenderer(ISender sender)
    : IArgusAnswerRenderer
{
    private const string EmailPlaceholder = "{{email}}";
    private const string CurrentPlaceholder =
        "{{current_exhibition}}";
    private const string NextPlaceholder =
        "{{next_exhibition}}";

    private static readonly CultureInfo Hungarian =
        CultureInfo.GetCultureInfo("hu-HU");

    private static readonly TimeZoneInfo Budapest =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Budapest");

    public async Task<string> RenderAsync(
        string answer,
        CancellationToken cancellationToken = default)
    {
        if (answer.Contains(
                CurrentPlaceholder,
                StringComparison.Ordinal) ||
            answer.Contains(
                NextPlaceholder,
                StringComparison.Ordinal))
        {
            IReadOnlyList<InvitationListItem> invitations =
                await sender.Send(
                    new GetInvitationsQuery(),
                    cancellationToken);

            DateTimeOffset now = DateTimeOffset.UtcNow;

            if (answer.Contains(
                    CurrentPlaceholder,
                    StringComparison.Ordinal))
            {
                InvitationListItem[] current = invitations
                    .Where(invitation =>
                        invitation.ExhibitionStartsAt.HasValue &&
                        invitation.ExhibitionEndsAt.HasValue &&
                        invitation.ExhibitionStartsAt.Value <= now &&
                        now < invitation.ExhibitionEndsAt.Value)
                    .OrderBy(invitation =>
                        invitation.ExhibitionEndsAt)
                    .ThenBy(invitation => invitation.Id)
                    .ToArray();

                string text = current.Length > 0
                    ? string.Join(
                        " ",
                        current.Select(DescribeExhibition))
                    : "A közzétett időpontok alapján jelenleg " +
                      "nem azonosítható biztosan látogatható " +
                      "kiállítás. További információért " +
                      "tekintse meg a meghívókat.";

                answer = answer.Replace(
                    CurrentPlaceholder,
                    text,
                    StringComparison.Ordinal);
            }

            if (answer.Contains(
                    NextPlaceholder,
                    StringComparison.Ordinal))
            {
                InvitationListItem[] upcoming = invitations
                    .Where(invitation =>
                        invitation.ExhibitionStartsAt.HasValue &&
                        invitation.ExhibitionStartsAt.Value > now)
                    .OrderBy(invitation =>
                        invitation.ExhibitionStartsAt)
                    .ThenBy(invitation => invitation.Id)
                    .ToArray();

                // Azonos kezdési időpontnál mindegyiket felsoroljuk.
                InvitationListItem[] next = upcoming.Length == 0
                    ? []
                    : upcoming
                        .TakeWhile(invitation =>
                            invitation.ExhibitionStartsAt ==
                            upcoming[0].ExhibitionStartsAt)
                        .ToArray();

                string text = next.Length > 0
                    ? string.Join(
                        " ",
                        next.Select(DescribeExhibition))
                    : "Jelenleg nincs közzétett kezdési időpont " +
                      "Henn László András következő kiállításához.";

                answer = answer.Replace(
                    NextPlaceholder,
                    text,
                    StringComparison.Ordinal);
            }
        }

        if (!answer.Contains(
                EmailPlaceholder,
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
            EmailPlaceholder,
            settings.EmailAddress.Trim(),
            StringComparison.Ordinal);
    }

    private static string DescribeExhibition(
        InvitationListItem invitation)
    {
        List<string> parts =
        [
            $"Kiállítás: „{invitation.TitleHu}”.",
        ];

        if (!string.IsNullOrWhiteSpace(invitation.LocationHu))
        {
            parts.Add(
                $"Helyszín: {invitation.LocationHu.Trim()}.");
        }

        if (invitation.ExhibitionStartsAt is { } startsAt)
        {
            parts.Add(
                $"Kezdés: {FormatDate(startsAt)}.");
        }

        if (invitation.ExhibitionEndsAt is { } endsAt)
        {
            parts.Add(
                $"Megtekinthető eddig: {FormatDate(endsAt)}.");
        }

        return string.Join(" ", parts);
    }

    private static string FormatDate(DateTimeOffset value)
    {
        return TimeZoneInfo.ConvertTime(value, Budapest)
            .ToString("yyyy. MMMM d. HH:mm", Hungarian);
    }
}