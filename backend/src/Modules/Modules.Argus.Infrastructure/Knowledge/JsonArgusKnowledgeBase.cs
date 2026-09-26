using System.Text.Json;
using Modules.Argus.Application.Abstractions;
using Modules.Argus.Domain.Knowledge;

namespace Modules.Argus.Infrastructure.Knowledge;

internal sealed class JsonArgusKnowledgeBase
    : IArgusKnowledgeBase
{
    private const string ResourceName =
        "Modules.Argus.Infrastructure." +
        "Knowledge.argus-knowledge.hu.json";

	private static readonly string
    KnowledgeBasePath =
        Path.Combine(
            AppContext.BaseDirectory,
            "Knowledge",
            "argus-knowledge.hu.json");

    private static readonly JsonSerializerOptions
        SerializerOptions =
            new()
            {
                PropertyNameCaseInsensitive = true
            };

    private readonly Lazy<
        Task<
            IReadOnlyCollection<
                ArgusKnowledgeEntry>>>
        _entries;


    public async Task<
        IReadOnlyCollection<ArgusKnowledgeEntry>>
        GetEntriesAsync(
            CancellationToken cancellationToken =
                default)
    {
        cancellationToken
            .ThrowIfCancellationRequested();

        return await _entries
            .Value
            .WaitAsync(cancellationToken);
    }

    private static async Task<
    IReadOnlyCollection<ArgusKnowledgeEntry>>
    LoadEntriesAsync()
	{
		if (!File.Exists(KnowledgeBasePath))
		{
			throw new InvalidOperationException(
				"Az Argus tudásbázisa nem " +
				$"található: {KnowledgeBasePath}");
		}

		await using FileStream stream =
			File.OpenRead(KnowledgeBasePath);

		List<ArgusKnowledgeEntry>? entries =
			await JsonSerializer
				.DeserializeAsync<
					List<ArgusKnowledgeEntry>>(
					stream,
					SerializerOptions);

		if (
			entries is null ||
			entries.Count == 0)
		{
			throw new InvalidOperationException(
				"Az Argus tudásbázisa üres.");
		}

		Validate(entries);

		return entries.AsReadOnly();
	}
	
    private static void Validate(
    IReadOnlyCollection<ArgusKnowledgeEntry>
            entries)
    {
        if (entries.Count == 0)
        {
            throw new InvalidOperationException(
                "Az Argus tudásbázisa üres.");
        }

        foreach (ArgusKnowledgeEntry entry
                in entries)
        {
            if (entry is null)
            {
                throw new InvalidOperationException(
                    "Az Argus tudásbázisa üres bejegyzést tartalmaz.");
            }

            if (string.IsNullOrWhiteSpace(entry.Id))
            {
                throw new InvalidOperationException(
                    "Az Argus egyik bejegyzéséből hiányzik az id.");
            }

            if (string.IsNullOrWhiteSpace(entry.Answer))
            {
                throw new InvalidOperationException(
                    $"Az '{entry.Id}' bejegyzésből hiányzik a válasz.");
            }

            if (
                entry.Questions is null ||
                entry.Questions.Count == 0 ||
                entry.Questions.Any(
                    string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException(
                    $"Az '{entry.Id}' bejegyzés nem tartalmaz " +
                    "érvényes kérdésmintát.");
            }

            if (
                entry.Keywords is null ||
                entry.Keywords.Count == 0 ||
                entry.Keywords.Any(
                    string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException(
                    $"Az '{entry.Id}' bejegyzés nem tartalmaz " +
                    "érvényes kulcsszót.");
            }

            bool hasSourceTitle =
                !string.IsNullOrWhiteSpace(
                    entry.SourceTitle);

            bool hasSourcePath =
                !string.IsNullOrWhiteSpace(
                    entry.SourcePath);

            if (hasSourceTitle != hasSourcePath)
            {
                throw new InvalidOperationException(
                    $"Az '{entry.Id}' bejegyzésnél a forrás címét " +
                    "és útvonalát együtt kell megadni.");
            }

            if (
                hasSourcePath &&
                !entry.SourcePath!.StartsWith(
                    "/",
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Az '{entry.Id}' forrásútvonalának '/' " +
                    "karakterrel kell kezdődnie.");
            }
        }

        string? duplicateId =
            entries
                .GroupBy(
                    entry => entry.Id,
                    StringComparer.Ordinal)
                .FirstOrDefault(
                    group => group.Count() > 1)
                ?.Key;

        if (duplicateId is not null)
        {
            throw new InvalidOperationException(
                $"Az Argus tudásbázisában ismétlődik az id: " +
                $"'{duplicateId}'.");
        }
    }

    private readonly Lazy<
    Task<ArgusKnowledgeDocument>>
    _knowledge;

    public JsonArgusKnowledgeBase()
    {
        _knowledge =
            new Lazy<Task<ArgusKnowledgeDocument>>(
                LoadAsync,
                LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public async Task<ArgusKnowledgeDocument> GetAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _knowledge
            .Value
            .WaitAsync(cancellationToken);
    }

    private static async Task<ArgusKnowledgeDocument>
        LoadAsync()
    {
        if (!File.Exists(KnowledgeBasePath))
        {
            throw new InvalidOperationException(
                "Az Argus tudásbázisa nem található: " +
                KnowledgeBasePath);
        }

        await using FileStream stream =
            File.OpenRead(KnowledgeBasePath);

        ArgusKnowledgeDocument? knowledge =
            await JsonSerializer.DeserializeAsync<
                ArgusKnowledgeDocument>(
                    stream,
                    SerializerOptions);

        if (knowledge is null)
        {
            throw new InvalidOperationException(
                "Az Argus tudásbázisa nem olvasható.");
        }

        ValidateSubject(knowledge.Subject);
        Validate(knowledge.Entries);

        return knowledge;
    }

    private static void ValidateSubject(
    ArgusSubject subject)
    {
        if (subject is null)
        {
            throw new InvalidOperationException(
                "Az Argus tudásbázisából hiányzik az alany.");
        }

        if (string.IsNullOrWhiteSpace(subject.Id))
        {
            throw new InvalidOperationException(
                "Az Argus alanyából hiányzik az id.");
        }

        if (string.IsNullOrWhiteSpace(
                subject.CanonicalName))
        {
            throw new InvalidOperationException(
                "Az Argus alanyából hiányzik a név.");
        }

        if (
            subject.Aliases is null ||
            subject.Aliases.Count == 0 ||
            subject.Aliases.Any(
                string.IsNullOrWhiteSpace))
        {
            throw new InvalidOperationException(
                "Az Argus alanya nem tartalmaz " +
                "érvényes névváltozatokat.");
        }

        bool containsCanonicalName =
            subject.Aliases.Any(alias =>
                string.Equals(
                    alias.Trim(),
                    subject.CanonicalName.Trim(),
                    StringComparison.OrdinalIgnoreCase));

        if (!containsCanonicalName)
        {
            throw new InvalidOperationException(
                "Az alany névváltozatai között a " +
                "teljes névnek is szerepelnie kell.");
        }
    }
}