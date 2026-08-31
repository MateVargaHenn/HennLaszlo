namespace Modules.FileStorage.Domain;

public sealed class StoredFile
{
    private StoredFile()
    {
    }

    private StoredFile(
        string originalFileName,
        string storageKey,
        string contentType,
        long sizeInBytes)
    {
        OriginalFileName = originalFileName;
        StorageKey = storageKey;
        ContentType = contentType;
        SizeInBytes = sizeInBytes;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string OriginalFileName { get; private set; } = string.Empty;

    public string StorageKey { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    public long SizeInBytes { get; private set; }

    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public static StoredFile Create(
        string originalFileName,
        string storageKey,
        string contentType,
        long sizeInBytes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(originalFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);

        if (sizeInBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeInBytes));
        }

        string safeOriginalFileName =
            Path.GetFileName(originalFileName);

        if (string.IsNullOrWhiteSpace(safeOriginalFileName))
        {
            throw new ArgumentException(
                "Érvénytelen fájlnév.",
                nameof(originalFileName));
        }

        return new StoredFile(
            safeOriginalFileName,
            storageKey.Trim(),
            contentType.Trim().ToLowerInvariant(),
            sizeInBytes);
    }
}