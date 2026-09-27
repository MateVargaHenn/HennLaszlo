using FluentValidation;

namespace Modules.Invitation.Application.Invitations.Publish;

internal sealed class PublishInvitationCommandValidator
    : AbstractValidator<PublishInvitationCommand>
{
    public PublishInvitationCommandValidator()
    {
        RuleFor(command => command.InvitationId)
            .NotEmpty();
    }
}