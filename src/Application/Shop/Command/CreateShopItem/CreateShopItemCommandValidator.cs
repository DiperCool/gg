using FluentValidation;

namespace CleanArchitecture.Application.Shop.Command.CreateShopItem;

public class CreateShopItemCommandValidator : AbstractValidator<CreateShopItemCommand>
{
    public CreateShopItemCommandValidator()
    {
        RuleFor(x => x.Price).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Specifications).NotEmpty();
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Description).NotEmpty();
    }
}