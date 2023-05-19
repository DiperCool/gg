using System.Data;
using CleanArchitecture.Application.Common.Validators;
using FluentValidation;

namespace CleanArchitecture.Application.Teams.Command.CreateTeam;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(20);
            //.Matches("/^[A-Za-z0-9]+$/");
        RuleFor(x => x.Tag)
            .MaximumLength(6)
            .Must(u => !u.Any(Char.IsWhiteSpace)).WithMessage("Whitespaces is disallowed");
        RuleFor(x => x.Picture)
            .SetValidator(new ImageValidator()!);
    }
}