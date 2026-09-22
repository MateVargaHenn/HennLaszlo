using FluentValidation;

namespace Modules.Invitation.Application.Invitations.AttachImage;

internal sealed class AttachInvitationImageCommandValidator
    : AbstractValidator<AttachInvitationImageCommand>
{
    public AttachInvitationImageCommandValidator()
    {
        RuleFor(command => command.InvitationId)
            .NotEmpty();

        RuleFor(command => command.FileId)
            .NotEmpty();
    }
}