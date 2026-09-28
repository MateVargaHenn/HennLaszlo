using FluentValidation;

namespace Modules.Video.Application.Videos.Unpublish;

internal sealed class UnpublishVideoCommandValidator
    : AbstractValidator<UnpublishVideoCommand>
{
    public UnpublishVideoCommandValidator()
    {
        RuleFor(command => command.VideoId)
            .NotEmpty();
    }
}