using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.Teams.Command.UpdateLogoTeam;

public class UpdateLogoTeamCommandValidator: AbstractValidator<UpdateLogoTeamCommand>
{
    public UpdateLogoTeamCommandValidator()
    {
        RuleFor(x => x.File)
            .SetValidator(new ImageValidator()!);
    }
}