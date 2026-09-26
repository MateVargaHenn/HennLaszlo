using Modules.Argus.Application.Abstractions;
using Modules.Argus.Application.Matching;
using Modules.Argus.Application.Models;
using Modules.Argus.Domain.Knowledge;

namespace Modules.Argus.Application.Answering;

public sealed class ArgusAnswerService(
    IArgusKnowledgeBase knowledgeBase)
    : IArgusAnswerService
{
    private const double MinimumConfidence =
        0.46;

    private const double StrongMatch =
        0.75;

    private const double MinimumLead =
        0.04;

    private const string FallbackAnswer =
        "Erre még nem tudok megbízható " +
        "választ adni. Próbáld meg másképp " +
        "megfogalmazni a kérdést.";

    private const int MaximumQuestionLength = 300;
    private const int MinimumQuestionLetterCount = 3;
    private const int MinimumStemLength = 5;
    private const double NegativeMatchThreshold =
    0.8;
    
    private static readonly char[] IntentSeparators =
    ['.', '!', '?', ';'];

    public async Task<ArgusAnswer> AskAsync(
        string question,
        CancellationToken cancellationToken =
            default)
    {
        if (!IsAcceptableQuestion(question))
        {
            return CreateFallback();
        }

        string[] intentSegments =
            SplitIntoIntentSegments(question);

        if (intentSegments.Length == 0)
        {
            return CreateFallback();
        }

        ArgusKnowledgeDocument knowledge =
            await knowledgeBase.GetAsync(
                cancellationToken);

        IReadOnlyCollection<ArgusKnowledgeEntry>
            entries =
                knowledge.Entries;

        var matchedCandidates =
            new List<Candidate>();

        foreach (string segment
                in intentSegments)
        {
            if (ContainsUnknownProperName(
                    segment,
                    knowledge))
            {
                return CreateFallback();
            }

            NormalizedText normalizedSegment =
                HungarianTextNormalizer.Normalize(
                    segment);

            if (string.IsNullOrEmpty(
                    normalizedSegment.Value))
            {
                continue;
            }

            if (ContainsUnsafeUnknownToken(
                    segment,
                    normalizedSegment,
                    knowledge))
            {
                return CreateFallback();
            }

            Candidate? match =
                FindBestCandidate(
                    normalizedSegment,
                    entries);

            if (match is null)
            {
                return CreateFallback();
            }

            bool alreadyMatched =
                matchedCandidates.Any(
                    candidate =>
                        string.Equals(
                            candidate.Entry.Id,
                            match.Entry.Id,
                            StringComparison.Ordinal));

            if (!alreadyMatched)
            {
                matchedCandidates.Add(match);
            }
        }

        if (matchedCandidates.Count == 0)
        {
            return CreateFallback();
        }

        string answer =
            string.Join(
                " ",
                matchedCandidates
                    .Select(candidate =>
                        candidate.Entry.Answer.Trim())
                    .Where(value =>
                        !string.IsNullOrWhiteSpace(
                            value))
                    .Distinct(
                        StringComparer.Ordinal));

        var sources =
            matchedCandidates
                .Where(candidate =>
                    !string.IsNullOrWhiteSpace(
                        candidate.Entry.SourceTitle) &&
                    !string.IsNullOrWhiteSpace(
                        candidate.Entry.SourcePath))
                .Select(candidate => (
                    Title:
                        candidate.Entry.SourceTitle,
                    Path:
                        candidate.Entry.SourcePath))
                .Distinct()
                .ToArray();

        /*
        * A jelenlegi válaszmodell csak egy forrást
        * támogat. Több különböző forrás esetén
        * inkább nem jelenítünk meg félrevezető linket.
        */
        string? sourceTitle =
            sources.Length == 1
                ? sources[0].Title
                : null;

        string? sourcePath =
            sources.Length == 1
                ? sources[0].Path
                : null;

        double confidence =
            matchedCandidates.Min(
                candidate =>
                    candidate.Score);

        return new ArgusAnswer(
            answer,
            Round(confidence),
            IsFallback: false,
            sourceTitle,
            sourcePath);
    }

    private static bool ContainsUnsafeUnknownToken(
    string originalQuestion,
    NormalizedText normalizedQuestion,
    ArgusKnowledgeDocument knowledge)
    {
        HashSet<string> knownTokens =
            CreateKnownTokens(knowledge);

        bool hasUnknownToken =
            normalizedQuestion.Tokens.Any(token =>
                !IsKnownToken(
                    token,
                    knownTokens));

        if (!hasUnknownToken)
        {
            return false;
        }

        /*
        * Ismeretlen szavakat akkor engedünk át,
        * ha a kérdés egyértelműen a támogatott
        * személyt nevezi meg.
        *
        * Ezután még a hasonlósági pontozásnak is
        * el kell érnie a szükséges küszöböt.
        */
        return !ContainsSupportedSubject(
            originalQuestion,
            knowledge.Subject);
    }

    private static bool ContainsSupportedSubject(
    string question,
    ArgusSubject subject)
    {
        NormalizedText normalizedQuestion =
            HungarianTextNormalizer.Normalize(
                question);

        IEnumerable<string> subjectExpressions =
    subject.Aliases
        .Concat(subject.References)
        .Concat(subject.Roles);

    return subjectExpressions
        .Select(
            HungarianTextNormalizer.Normalize)
        .Where(expression =>
            expression.Tokens.Count > 0)
        .Any(expression =>
            expression.Tokens.All(expressionToken =>
                normalizedQuestion.Tokens.Any(
                    questionToken =>
                        IsSubjectTokenMatch(
                            expressionToken,
                            questionToken))));
    }

    private static bool IsSubjectTokenMatch(
        string aliasToken,
        string questionToken)
    {
        if (string.Equals(
                aliasToken,
                questionToken,
                StringComparison.Ordinal))
        {
            return true;
        }

        const int minimumPrefixLength = 4;

        if (
            aliasToken.Length <
                minimumPrefixLength ||
            questionToken.Length <
                minimumPrefixLength)
        {
            return false;
        }

        return string.Equals(
            aliasToken[
                ..minimumPrefixLength],
            questionToken[
                ..minimumPrefixLength],
            StringComparison.Ordinal);
    }

    private static string[] SplitIntoIntentSegments(
    string question)
    {
        return question.Split(
            IntentSeparators,
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);
    }

    private Candidate? FindBestCandidate(
        NormalizedText normalizedQuestion,
        IReadOnlyCollection<ArgusKnowledgeEntry>
            entries)
    {
        Candidate[] candidates =
            entries
                .Select(entry =>
                    new Candidate(
                        entry,
                        CalculateScore(
                            normalizedQuestion,
                            entry)))
                .OrderByDescending(
                    candidate =>
                        candidate.Score)
                .ToArray();

        if (candidates.Length == 0)
        {
            return null;
        }

        Candidate best =
            candidates[0];

        if (best.Score < MinimumConfidence)
        {
            return null;
        }

        if (
            candidates.Length > 1 &&
            best.Score < StrongMatch &&
            best.Score - candidates[1].Score <
                MinimumLead)
        {
            return null;
        }

        return best;
    }

    private static bool IsAcceptableQuestion(
    string? question)
    {
        if (
            string.IsNullOrWhiteSpace(question) ||
            question.Length > MaximumQuestionLength)
        {
            return false;
        }

        int letterCount =
            question.Count(char.IsLetter);

        if (letterCount <
            MinimumQuestionLetterCount)
        {
            return false;
        }

        return !question.Any(char.IsControl);
    }

    private static bool ContainsUnknownMeaningfulToken(
        NormalizedText question,
        ArgusKnowledgeDocument knowledge)
    {
        HashSet<string> knownTokens =
            CreateKnownTokens(knowledge);

        return question.Tokens.Any(token =>
            !IsKnownToken(
                token,
                knownTokens));
    }

    

    private static void AddKnownTokens(
    HashSet<string> knownTokens,
    string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        NormalizedText normalized =
            HungarianTextNormalizer.Normalize(
                text);

        foreach (string token
                in SplitTokens(normalized.Value))
        {
            knownTokens.Add(token);
        }
    }


    private static bool IsKnownToken(
    string token,
    HashSet<string> knownTokens)
    {
        if (knownTokens.Contains(token))
        {
            return true;
        }

        if (token.Length < MinimumStemLength)
        {
            return false;
        }

        string stem =
            token[..MinimumStemLength];

        return knownTokens.Any(
            knownToken =>
                knownToken.Length >=
                    MinimumStemLength &&
                knownToken.StartsWith(
                    stem,
                    StringComparison.Ordinal));
    }

    private static string[] SplitTokens(
        string text)
    {
        return text.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);
    }

    private static bool ContainsUnknownProperName(
        string question,
        ArgusKnowledgeDocument knowledge)
    {
        HashSet<string> knownTokens =
            CreateKnownTokens(knowledge);

        string[] words =
            question.Split(
                [' ', '\t', '\r', '\n'],
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);

        for (int index = 0;
            index < words.Length;
            index++)
        {
            string word = words[index];

            char firstLetter =
                word.FirstOrDefault(char.IsLetter);

            if (
                firstLetter == default ||
                !char.IsUpper(firstLetter))
            {
                continue;
            }

            NormalizedText normalized =
                HungarianTextNormalizer.Normalize(word);

            foreach (string token
                    in normalized.Tokens)
            {
                if (!IsKnownToken(
                        token,
                        knownTokens))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static HashSet<string> CreateKnownTokens(
    ArgusKnowledgeDocument knowledge)
    {
        var knownTokens =
            new HashSet<string>(
                StringComparer.Ordinal);

        AddKnownTokens(
            knownTokens,
            knowledge.Subject.CanonicalName);

        foreach (string alias
                in knowledge.Subject.Aliases)
        {
            AddKnownTokens(
                knownTokens,
                alias);
        }

        foreach (string reference
                in knowledge.Subject.References)
        {
            AddKnownTokens(
                knownTokens,
                reference);
        }

        foreach (string role
                in knowledge.Subject.Roles)
        {
            AddKnownTokens(
                knownTokens,
                role);
        }

        foreach (ArgusEntity entity
                in knowledge.Entities)
        {
            AddKnownTokens(
                knownTokens,
                entity.CanonicalName);

            foreach (string alias
                    in entity.Aliases)
            {
                AddKnownTokens(
                    knownTokens,
                    alias);
            }
        }

        foreach (ArgusKnowledgeEntry entry
                in knowledge.Entries)
        {
            AddKnownTokens(
                knownTokens,
                entry.Answer);

            AddKnownTokens(
                knownTokens,
                entry.SourceTitle);

            foreach (string question
                    in entry.Questions)
            {
                AddKnownTokens(
                    knownTokens,
                    question);
            }

            foreach (string keyword
                    in entry.Keywords)
            {
                AddKnownTokens(
                    knownTokens,
                    keyword);
            }

            foreach (string concept
                    in entry.Concepts)
            {
                AddKnownTokens(
                    knownTokens,
                    concept);
            }
        }

        return knownTokens;
    }

    private static bool IsKnownToken(
        string token,
        IReadOnlySet<string> knownTokens)
    {
        if (knownTokens.Contains(token))
        {
            return true;
        }

        return knownTokens.Any(knownToken =>
            token.Length >= 5 &&
            knownToken.Length >= 5 &&
            string.Equals(
                token[..5],
                knownToken[..5],
                StringComparison.Ordinal));
    }

    private static ArgusAnswer CreateFallback(
        string answer = FallbackAnswer)
    {
        return new ArgusAnswer(
            answer,
            Confidence: 0,
            IsFallback: true,
            SourceTitle: null,
            SourcePath: null);
    }

    private static double CalculateScore(
    NormalizedText question,
    ArgusKnowledgeEntry entry)
    {
        double negativeScore =
            entry.NegativeExamples
                .Select(
                    HungarianTextNormalizer.Normalize)
                .Select(candidate =>
                    TextSimilarity.Calculate(
                        question,
                        candidate))
                .DefaultIfEmpty()
                .Max();

        if (negativeScore >=
            NegativeMatchThreshold)
        {
            return 0;
        }

        IEnumerable<string> patterns =
            entry.Questions
                .Concat(entry.Concepts);

        double questionScore =
            patterns
                .Select(
                    HungarianTextNormalizer.Normalize)
                .Select(candidate =>
                    TextSimilarity.Calculate(
                        question,
                        candidate))
                .DefaultIfEmpty()
                .Max();

        NormalizedText keywords =
            HungarianTextNormalizer.Normalize(
                string.Join(
                    ' ',
                    entry.Keywords));

        double keywordScore =
            TextSimilarity.Calculate(
                question,
                keywords);

        return
            questionScore * 0.9 +
            keywordScore * 0.1;
    }

    private static ArgusAnswer
        CreateFallback()
    {
        return new ArgusAnswer(
            FallbackAnswer,
            Confidence: 0,
            IsFallback: true,
            SourceTitle: null,
            SourcePath: null);
    }

    private static double Round(
        double confidence)
    {
        return Math.Round(
            confidence,
            3,
            MidpointRounding.AwayFromZero);
    }

    private sealed record Candidate(
        ArgusKnowledgeEntry Entry,
        double Score);
}