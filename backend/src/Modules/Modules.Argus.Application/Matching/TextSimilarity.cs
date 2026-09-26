namespace Modules.Argus.Application.Matching;

internal static class TextSimilarity
{
    internal static double Calculate(
        NormalizedText left,
        NormalizedText right)
    {
        if (
            string.IsNullOrEmpty(left.Value) ||
            string.IsNullOrEmpty(right.Value))
        {
            return 0;
        }

        if (string.Equals(
                left.Value,
                right.Value,
                StringComparison.Ordinal))
        {
            return 1;
        }

        double trigramScore =
            CalculateTrigramDice(
                left.Value,
                right.Value);

        double tokenScore =
            CalculateTokenDice(
                left.Tokens,
                right.Tokens);

        return
            trigramScore * 0.65 +
            tokenScore * 0.35;
    }

    private static double CalculateTrigramDice(
        string left,
        string right)
    {
        HashSet<string> leftTrigrams =
            CreateTrigrams(left);

        HashSet<string> rightTrigrams =
            CreateTrigrams(right);

        int intersection =
            leftTrigrams.Count(
                rightTrigrams.Contains);

        return
            2d * intersection /
            (
                leftTrigrams.Count +
                rightTrigrams.Count
            );
    }

    private static double CalculateTokenDice(
        IReadOnlySet<string> left,
        IReadOnlySet<string> right)
    {
        if (
            left.Count == 0 ||
            right.Count == 0)
        {
            return 0;
        }

        int intersection =
            left.Count(right.Contains);

        return
            2d * intersection /
            (left.Count + right.Count);
    }

    private static HashSet<string>
        CreateTrigrams(string value)
    {
        string padded = $"  {value}  ";

        var trigrams =
            new HashSet<string>(
                StringComparer.Ordinal);

        for (
            int index = 0;
            index <= padded.Length - 3;
            index++)
        {
            trigrams.Add(
                padded.Substring(index, 3));
        }

        return trigrams;
    }
}