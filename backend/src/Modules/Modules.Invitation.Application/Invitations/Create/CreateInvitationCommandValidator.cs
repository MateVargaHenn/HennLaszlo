using FluentValidation;

namespace Modules.Invitation.Application.Invitations.Create;

internal sealed class CreateInvitationCommandValidator
    : AbstractValidator<CreateInvitationCommand>
{
    public CreateInvitationCommandValidator()
    {
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

        RuleFor(command => command.ExhibitionEndsAt)
            .Must((command, endsAt) =>
                !command.ExhibitionStartsAt.HasValue ||
                !endsAt.HasValue ||
                endsAt.Value >= command.ExhibitionStartsAt.Value)
            .WithMessage(
                "A kiállítás vége nem lehet korábbi a kezdeténél.");

        RuleFor(command => command.LocationHu)
            .MaximumLength(500);

        RuleFor(command => command.LocationEn)
            .MaximumLength(500);

        RuleFor(command => command.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}