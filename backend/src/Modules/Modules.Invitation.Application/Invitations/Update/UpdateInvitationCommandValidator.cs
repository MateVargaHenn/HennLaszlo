using FluentValidation;

namespace Modules.Invitation.Application.Invitations.Update;

internal sealed class UpdateInvitationCommandValidator
    : AbstractValidator<UpdateInvitationCommand>
{
    public UpdateInvitationCommandValidator()
    {
        RuleFor(command => command.InvitationId)
            .NotEmpty();

        RuleFor(command => command.TitleHu)
            .NotEmpty()
            .WithMessage(
                "A magyar cím megadása kötelező.")
            .MaximumLength(250);

        RuleFor(command => command.TitleEn)
            .MaximumLength(250);

        RuleFor(command => command.Year)
            .GreaterThan(0)
            .LessThanOrEqualTo(
                DateTime.UtcNow.Year + 1)
            .When(command => command.Year.HasValue);

        RuleFor(command => command.AltTextHu)
            .MaximumLength(500);

        RuleFor(command => command.AltTextEn)
            .MaximumLength(500);

        RuleFor(command => command.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}