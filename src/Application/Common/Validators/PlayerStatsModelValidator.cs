using FluentValidation;

namespace CleanArchitecture.Application.Common.Validators;

public class PlayerStatsModelValidator : AbstractValidator<PlayerStatsModel>
{
    public PlayerStatsModelValidator()
    {
        RuleFor(x => x.Kills).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Place).GreaterThanOrEqualTo(1);
    }
}