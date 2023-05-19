using FluentValidation;

namespace CleanArchitecture.Application.Shop.Query.GetShopItems;

public class GetShopItemsQueryValidator : AbstractValidator<GetShopItemsQuery>
{
    public GetShopItemsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
    }
}