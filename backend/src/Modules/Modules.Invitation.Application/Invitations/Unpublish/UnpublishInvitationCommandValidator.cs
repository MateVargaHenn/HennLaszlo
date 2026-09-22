using FluentValidation;

namespace Modules.Invitation.Application.Invitations.Unpublish;

internal sealed class UnpublishInvitationCommandValidator
    : AbstractValidator<UnpublishInvitationCommand>
{
    public UnpublishInvitationCommandValidator()
    {
        RuleFor(command => command.InvitationId)
            .NotEmpty();
    }
}