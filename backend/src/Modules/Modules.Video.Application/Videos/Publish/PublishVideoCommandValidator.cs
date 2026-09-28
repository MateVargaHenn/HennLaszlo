using FluentValidation;

namespace Modules.Video.Application.Videos.Publish;

internal sealed class PublishVideoCommandValidator
    : AbstractValidator<PublishVideoCommand>
{
    public PublishVideoCommandValidator()
    {
        RuleFor(command => command.VideoId)
            .NotEmpty();
    }
}