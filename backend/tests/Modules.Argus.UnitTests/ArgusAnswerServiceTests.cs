using Modules.Argus.Application.Abstractions;
using Modules.Argus.Application.Answering;
using Modules.Argus.Application.Models;
using Modules.Argus.Domain.Knowledge;

namespace Modules.Argus.UnitTests;

public sealed class ArgusAnswerServiceTests
{
    private readonly ArgusAnswerService
        _service =
            new(
                new StubKnowledgeBase());

    [Theory]
    [InlineData("Ki Henn Laszlo Andras?")]
    [InlineData(
        "Mivel foglalkozik Henn László?")]
    [InlineData(
        "Milyen művész Henn László András?")]
    public async Task AskAsync_ReturnsAnswer_ForKnownQuestion(
        string question)
    {
        ArgusAnswer result =
            await _service.AskAsync(question);

        Assert.False(result.IsFallback);

        Assert.Equal(
            "Henn László András " +
            "festőművész és grafikus.",
            result.Answer);

        Assert.InRange(
            result.Confidence,
            0.46,
            1);

        Assert.Equal(
            "/bemutatkozas",
            result.SourcePath);
    }

    [Fact]
    public async Task AskAsync_ReturnsFallback_ForUnknownQuestion()
    {
        ArgusAnswer result =
            await _service.AskAsync(
                "Milyen idő lesz holnap Budapesten?");

        Assert.True(result.IsFallback);
        Assert.Equal(0, result.Confidence);
        Assert.Null(result.SourcePath);
    }

    [Fact]
    public async Task AskAsync_ReturnsFallback_ForEmptyQuestion()
    {
        ArgusAnswer result =
            await _service.AskAsync(" ");

        Assert.True(result.IsFallback);
    }

    private sealed class StubKnowledgeBase
        : IArgusKnowledgeBase
    {
        public Task<ArgusKnowledgeDocument>
            GetAsync(
                CancellationToken cancellationToken =
                    default)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            IReadOnlyCollection<
                ArgusKnowledgeEntry> entries =
            [
                new ArgusKnowledgeEntry(
                    Id: "identity",
                    Answer:
                        "Henn László András " +
                        "festőművész és grafikus.",
                    SourceTitle: "Bemutatkozás",
                    SourcePath: "/bemutatkozas",
                    Questions:
                    [
                        "Ki Henn László András?",
                        "Ki Henn Laszlo Andras?",
                        "Mivel foglalkozik Henn László?",
                        "Milyen művész Henn László András?"
                    ],
                    Keywords:
                    [
                        "Henn László András",
                        "festőművész",
                        "grafikus",
                        "alkotó"
                    ])
            ];

            var subject =
                new ArgusSubject(
                    Id: "henn-laszlo-andras",
                    CanonicalName:
                        "Henn László András",
                    Aliases:
                    [
                        "Henn László András",
                        "Henn László",
                        "Henn"
                    ]);

            var knowledge =
                new ArgusKnowledgeDocument(
                    subject,
                    entries);

            return Task.FromResult(
                knowledge);
        }
    }
}