using FluentValidation;

namespace CleanArchitecture.Application.Teams.Command.JoinManagerTeam;

public class JoinManagerTeamCommandValidator: AbstractValidator<JoinManagerTeamCommand>
{
    public JoinManagerTeamCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}