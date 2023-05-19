using FluentValidation;

namespace CleanArchitecture.Application.Common.Validators;

public class StatisticModelsValidator: AbstractValidator<UserStatisticModel>
{
    public StatisticModelsValidator()
    {
        RuleFor(x => x.Kills)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Place)
            .GreaterThanOrEqualTo(1);
    }
}