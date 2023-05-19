using FluentValidation;

namespace CleanArchitecture.Application.Common.Validators;

public class StageWithWinnersModelValidator : AbstractValidator<StageWithWinnersModel>
{
    public StageWithWinnersModelValidator()
    {
        
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.StageStart).GreaterThanOrEqualTo(DateTime.UtcNow);
        RuleForEach(x => x.Groups).SetValidator(new GroupValidator());
    }
}