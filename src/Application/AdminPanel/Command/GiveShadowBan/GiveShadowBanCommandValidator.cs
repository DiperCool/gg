using FluentValidation;

namespace CleanArchitecture.Application.AdminPanel.Command.GiveShadowBan;

public class GiveShadowBanCommandValidator: AbstractValidator<GiveShadowBanCommand>
{
    public GiveShadowBanCommandValidator()
    {
        RuleFor(x => x.To).GreaterThan(DateTime.UtcNow);
    }
}