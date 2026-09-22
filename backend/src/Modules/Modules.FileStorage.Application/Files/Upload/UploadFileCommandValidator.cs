using FluentValidation;

namespace Modules.FileStorage.Application.Files.Upload;

internal sealed class UploadFileCommandValidator
    : AbstractValidator<UploadFileCommand>
{
    private const long MaximumFileSize =
        25 * 1024 * 1024;

    private static readonly Dictionary<string, string[]>
        AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            ["image/jpeg"] = [".jpg", ".jpeg"],
            ["image/png"] = [".png"],
            ["image/webp"] = [".webp"],
            ["image/avif"] = [".avif"],
            ["application/pdf"] = [".pdf"]
        };

    public UploadFileCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .Must(AllowedTypes.ContainsKey)
            .WithMessage("A megadott fájltípus nem támogatott.");

        RuleFor(x => x.SizeInBytes)
            .GreaterThan(0)
            .LessThanOrEqualTo(MaximumFileSize)
            .WithMessage(
                "A fájl mérete legfeljebb 25 MB lehet.");

        RuleFor(x => x.Content)
            .NotNull()
            .Must(stream => stream.CanRead)
            .WithMessage("A fájl tartalma nem olvasható.");

        RuleFor(x => x)
            .Must(HaveMatchingExtension)
            .WithMessage(
                "A fájl kiterjesztése nem egyezik a tartalomtípussal.");
    }

    private static bool HaveMatchingExtension(
        UploadFileCommand command)
    {
        if (!AllowedTypes.TryGetValue(
                command.ContentType,
                out string[]? extensions))
        {
            return false;
        }

        string extension =
            Path.GetExtension(command.FileName);

        return extensions.Contains(
            extension,
            StringComparer.OrdinalIgnoreCase);
    }
}