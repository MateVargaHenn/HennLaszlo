using Modules.FileStorage.Application.Abstractions;

namespace Modules.FileStorage.Infrastructure.Storage;

internal sealed class LocalFileContentStorage(
    string rootPath)
    : IFileContentStorage
{
    private readonly string _rootPath =
        Path.GetFullPath(rootPath);

    public async Task<string> SaveAsync(
        Stream content,
        string extension,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);

        string safeExtension = NormalizeExtension(extension);

        DateTime now = DateTime.UtcNow;

        string storageKey = Path.Combine(
                now.Year.ToString(),
                now.Month.ToString("00"),
                $"{Guid.NewGuid():N}{safeExtension}")
            .Replace('\\', '/');

        string fullPath = GetFullPath(storageKey);

        Directory.CreateDirectory(
            Path.GetDirectoryName(fullPath)!);

        try
        {
            await using var outputStream = new FileStream(
                fullPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                FileOptions.Asynchronous);

            await content.CopyToAsync(
                outputStream,
                cancellationToken);

            return storageKey;
        }
        catch
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            throw;
        }
    }

    public Task<Stream?> OpenReadAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        string fullPath = GetFullPath(storageKey);

        if (!File.Exists(fullPath))
        {
            return Task.FromResult<Stream?>(null);
        }

        Stream stream = new FileStream(
            fullPath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 81920,
            FileOptions.Asynchronous |
            FileOptions.SequentialScan);

        return Task.FromResult<Stream?>(stream);
    }

    public Task DeleteAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string fullPath = GetFullPath(storageKey);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    private string GetFullPath(string storageKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);

        string fullPath = Path.GetFullPath(
            Path.Combine(_rootPath, storageKey));

        string allowedPrefix =
            _rootPath.TrimEnd(Path.DirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        if (!fullPath.StartsWith(
                allowedPrefix,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "A tárolási kulcs érvénytelen.");
        }

        return fullPath;
    }

    private static string NormalizeExtension(string extension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);

        string normalized =
            "." + extension.Trim().TrimStart('.').ToLowerInvariant();

        if (normalized.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
            normalized.Contains('/') ||
            normalized.Contains('\\'))
        {
            throw new ArgumentException(
                "Érvénytelen fájlkiterjesztés.",
                nameof(extension));
        }

        return normalized;
    }
}