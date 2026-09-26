using System.Collections.Frozen;
using System.Globalization;
using System.Text;

namespace Modules.Argus.Application.Matching;

internal sealed record NormalizedText(
    string Value,
    IReadOnlySet<string> Tokens);

internal static class HungarianTextNormalizer
{
    private static readonly FrozenSet<string>
        StopWords =
            new[]
            {
                "a",
                "az",
                "egy",
                "es",
                "hogy",
                "is",
                "meg",
                "vagy",
                "de",
                "ha",
                "akkor",
                "aki",
                "ami",
                "amely",
                "ennek",
                "annak",
                "ez",
                "azt",
                "ezt",
                "o",
                "ot",
                "neki",
                "rola",
                "szamara",
                "valamint",
                "illetve",
                "pedig",
                "vajon",
                "pontosan",
                "kerlek",
                "legyszives",
                "tulajdonkeppen",
            }
            .ToFrozenSet(
                StringComparer.Ordinal);

    internal static NormalizedText Normalize(
        string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return new NormalizedText(
                string.Empty,
                new HashSet<string>(
                    StringComparer.Ordinal));
        }

        string decomposed =
            text.Normalize(
                NormalizationForm.FormD);

        var builder =
            new StringBuilder(
                decomposed.Length);

        foreach (char character in decomposed)
        {
            UnicodeCategory category =
                CharUnicodeInfo.GetUnicodeCategory(
                    character);

            if (category ==
                UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (char.IsLetterOrDigit(character))
            {
                builder.Append(
                    char.ToLowerInvariant(
                        character));
            }
            else
            {
                builder.Append(' ');
            }
        }

        string[] tokens =
            builder
                .ToString()
                .Split(
                    ' ',
                    StringSplitOptions
                        .RemoveEmptyEntries |
                    StringSplitOptions
                        .TrimEntries)
                .Where(token =>
                    !StopWords.Contains(token))
                .ToArray();

        return new NormalizedText(
            string.Join(' ', tokens),
            tokens.ToHashSet(
                StringComparer.Ordinal));
    }
}