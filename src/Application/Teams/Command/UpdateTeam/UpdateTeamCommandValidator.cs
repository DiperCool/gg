using FluentValidation;

namespace CleanArchitecture.Application.Teams.Command.UpdateTeam;

public class UpdateTeamCommandValidator: AbstractValidator<UpdateTeamCommand>
{
    public UpdateTeamCommandValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(20);
        RuleFor(x => x.Tag)
            .MaximumLength(6)
            .Must(u => !u.Any(Char.IsWhiteSpace)).WithMessage("Whitespaces is disallowed");
    }
}