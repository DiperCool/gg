using FluentValidation;

namespace CleanArchitecture.Application.Shop.Command.UpdateShopItem;

public class UpdateShopItemCommandValidator : AbstractValidator<UpdateShopItemCommand>
{
    public UpdateShopItemCommandValidator()
    {
        RuleFor(x => x.Price).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Specifications).NotEmpty();
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Description).NotEmpty();
    }
}