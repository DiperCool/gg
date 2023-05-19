using FluentValidation;

namespace CleanArchitecture.Application.Teams.Command.JoinTeam;

public class JoinTeamCommandValidator: AbstractValidator<JoinTeamCommand>
{
    public JoinTeamCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}