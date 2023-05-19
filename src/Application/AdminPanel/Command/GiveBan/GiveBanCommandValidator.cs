using FluentValidation;

namespace CleanArchitecture.Application.AdminPanel.Command.GiveBan;

public class GiveBanCommandValidator : AbstractValidator<GiveBanCommand>
{
    public GiveBanCommandValidator()
    {
        RuleFor(x => x.To).GreaterThan(DateTime.UtcNow);
    }
}